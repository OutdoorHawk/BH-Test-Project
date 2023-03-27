using System.Collections.Generic;
using Mirror;
using MirrorServiceTest.Code.Infrastructure.Data;
using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Infrastructure.Services.Network;
using MirrorServiceTest.Code.Infrastructure.Services.UI;
using MirrorServiceTest.Code.Infrastructure.Services.UpdateBehavior;
using MirrorServiceTest.Code.Infrastructure.StateMachine;
using MirrorServiceTest.Code.Infrastructure.StateMachine.States;
using MirrorServiceTest.Code.Runtime.Animation;
using MirrorServiceTest.Code.Runtime.CameraLogic;
using MirrorServiceTest.Code.Runtime.Player.Input;
using MirrorServiceTest.Code.Runtime.Player.StateMachine;
using MirrorServiceTest.Code.Runtime.Player.StateMachine.States;
using MirrorServiceTest.Code.Runtime.Player.Systems;
using MirrorServiceTest.Code.Runtime.Player.UI;
using MirrorServiceTest.Code.Runtime.Player.UI.TimeControlMenu;
using MirrorServiceTest.Code.StaticData;
using UnityEngine;

namespace MirrorServiceTest.Code.Runtime.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerCollisionDetector))]
    [RequireComponent(typeof(ColorChangeComponent))]
    public class PlayerBehavior : NetworkBehaviour
    {
        [SerializeField] private CameraFollow _cameraFollowPrefab;
        [SyncVar] private PlayerStaticData _playerStaticData;

        private CameraFollow _cameraFollow;
        private PlayerInput _playerInput;
        private PlayerAnimator _animator;
        private PlayerCollisionDetector _collisionDetector;
        private PlayerGameStatus _playerGameStatus;
        private PlayerHUD _playerHUD;
        private IUIFactory _uiFactory;
        private IGameNetworkService _networkService;
        private IGameStateMachine _gameStateMachine;
        private WorldStaticData _worldStaticData;
        private IUpdateBehaviourService _updateBehavior;
        
        public TimeControlHUD TimeControl { get; private set; }
        public PlayerMovement Movement { get; private set; }
        public IPlayerStateMachine StateMachine { get; private set; }

        [ClientRpc]
        public void RpcConstruct(PlayerStaticData staticData, WorldStaticData worldStaticData)
        {
            _worldStaticData = worldStaticData;
            _playerStaticData = staticData;
            _uiFactory = DIContainer.Container.Resolve<IUIFactory>();
            _networkService = DIContainer.Container.Resolve<IGameNetworkService>();
            _gameStateMachine = DIContainer.Container.Resolve<IGameStateMachine>();
            _updateBehavior = DIContainer.Container.Resolve<IUpdateBehaviourService>();
        }

        [ClientRpc]
        public void RpcInitializePlayer()
        {
            if (!isOwned)
                return;
            CreateSystems();
            InitSystems();
            CmdAskForScoreTableUpdate();
        }

        private void CreateSystems()
        {
            Animator animator = GetComponent<Animator>();
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            ColorChangeComponent changeComponent = GetComponent<ColorChangeComponent>();
            _collisionDetector = GetComponent<PlayerCollisionDetector>();
            _playerHUD = _uiFactory.CreatePlayerHUD(connectionToClient);
            TimeControl = _uiFactory.CreateTimeControl(connectionToClient);
            _playerInput = new PlayerInput();
            _animator = new PlayerAnimator(animator);
            _cameraFollow = Instantiate(_cameraFollowPrefab);
            Movement =
                new PlayerMovement(_playerStaticData, rigidbody, transform, _cameraFollow, this);
            _playerGameStatus = new PlayerGameStatus(_playerStaticData, this, changeComponent);
            StateMachine =
                new PlayerStateMachine(Movement, _playerInput, _animator, _collisionDetector,
                    _playerGameStatus, this, _playerStaticData);
        }

        private void InitSystems()
        {
            _playerInput.Init();
            _playerInput.EnableAllInput();
            _cameraFollow.Init(_playerInput, _playerStaticData, transform, _updateBehavior);
            StateMachine.Enter<BasicMovementState>();
            _playerHUD.Init(_worldStaticData.GameRestartDelay, _playerInput);
            TimeControl.Init(_playerInput);
            _playerGameStatus.OnPlayerHit += CmdAskForPlayerHit;
            _playerHUD.OnDisconnectButtonPressed += DisconnectFromGame;
            _updateBehavior.UpdateEvent += Tick;
            _updateBehavior.FixedUpdateEvent += FixedTick;
        }

        [Command]
        private void CmdAskForScoreTableUpdate()
        {
            _networkService.UpdatePlayersHUD();
        }

        [TargetRpc]
        public void TargetUpdateHUD(List<PlayerProfile> profiles)
        {
            _playerHUD.UpdateScoreTable(profiles);
        }

        private void Tick()
        {
            if (!isOwned)
                return;

            StateMachine?.Tick();
        }

        private void FixedTick()
        {
            if (!isOwned)
                return;
            StateMachine?.FixedTick();
        }

        [Command]
        private void CmdAskForPlayerHit(NetworkIdentity target)
        {
            int targetID = target.connectionToClient.connectionId;
            int senderID = connectionToClient.connectionId;
            _networkService.SendHitToPlayer(targetID, senderID);
        }

        [TargetRpc]
        public void TargetPlayerHit(int senderID)
        {
            if (_playerGameStatus.IsHitNow)
                return;

            StateMachine.Enter<BasicMovementState>();
            _playerGameStatus.PlayerHit();
            CmdSuccessHit(senderID);
        }

        [Command]
        private void CmdSuccessHit(int senderID)
        {
            _networkService.SendHitSuccess(senderID);
        }

        [TargetRpc]
        public void TargetGameEnd(string winnerName)
        {
            _playerHUD.EnableEndGamePanel(winnerName);
            StateMachine.Enter<EndGameState>();
        }

        private void DisconnectFromGame()
        {
            _gameStateMachine.Enter<MainMenuState>();
            NetworkClient.Disconnect();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            if (!isOwned)
                return;

            _gameStateMachine.Enter<MainMenuState>();
        }

        public override void OnStopLocalPlayer()
        {
            DisposePlayer();
            base.OnStopLocalPlayer();
        }

        private void DisposePlayer()
        {
            _updateBehavior.FixedUpdateEvent -= FixedTick;
            _updateBehavior.UpdateEvent -= Tick;
            _playerHUD.OnDisconnectButtonPressed -= DisconnectFromGame;
            _playerGameStatus.OnPlayerHit -= CmdAskForPlayerHit;
            StateMachine.CleanUp();
            _playerInput.CleanUp();
            _animator.CleanUp();
        }
    }
}
using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.UI;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;
using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.CameraLogic;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.StateMachine;
using BH_Test_Project.Code.Runtime.Player.StateMachine.States;
using BH_Test_Project.Code.Runtime.Player.Systems;
using BH_Test_Project.Code.Runtime.Player.UI;
using BH_Test_Project.Code.StaticData;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerCollisionDetector))]
    [RequireComponent(typeof(ColorChangeComponent))]
    [RequireComponent(typeof(PlayerNameComponent))]
    public class PlayerBehavior : NetworkBehaviour
    {
        [SerializeField] private CameraFollow _cameraFollowPrefab;
        [SerializeField, SyncVar] private PlayerStaticData _playerStaticData;

        private CameraFollow _cameraFollow;
        private PlayerInput _playerInput;
        private PlayerMovement _playerMovement;
        private PlayerAnimator _animator;
        private PlayerCollisionDetector _collisionDetector;
        private PlayerGameStatus _playerGameStatus;
        private IPlayerStateMachine _playerStateMachine;
        private PlayerNameComponent _playerNameComponent;
        private PlayerHUD _playerHUD;
        private IUIFactory _uiFactory;
        private IGameNetworkService _networkService;
        private IGameStateMachine _gameStateMachine;

        [ClientRpc]
        public void RpcConstruct(PlayerStaticData staticData)
        {
            _playerStaticData = staticData;
            _uiFactory = DIContainer.Container.Resolve<IUIFactory>();
            _networkService = DIContainer.Container.Resolve<IGameNetworkService>();
            _gameStateMachine = DIContainer.Container.Resolve<IGameStateMachine>();
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
            CharacterController characterController = GetComponent<CharacterController>();
            ColorChangeComponent changeComponent = GetComponent<ColorChangeComponent>();
            _collisionDetector = GetComponent<PlayerCollisionDetector>();
            _playerNameComponent = GetComponent<PlayerNameComponent>();
            _playerHUD = _uiFactory.CreatePlayerHUD(connectionToClient);
            _playerInput = new PlayerInput();
            _animator = new PlayerAnimator(animator);
            _cameraFollow = Instantiate(_cameraFollowPrefab);
            _playerMovement =
                new PlayerMovement(_playerStaticData, characterController, transform, _cameraFollow, this);
            _playerGameStatus = new PlayerGameStatus(_playerStaticData, this, changeComponent);
            _playerStateMachine =
                new PlayerStateMachine(_playerMovement, _playerInput, _animator, _collisionDetector, netId, this,
                    _playerStaticData);
        }

        private void InitSystems()
        {
            _playerInput.Init();
            _playerInput.EnableAllInput();
            _cameraFollow.Init(_playerInput, _playerStaticData, transform);
            _playerStateMachine.Enter<BasicMovementState>();
            _playerHUD.Init(5);
            _playerHUD.OnDisconnectButtonPressed += DisconnectFromGame;
        }

        [Command]
        private void CmdAskForScoreTableUpdate()
        {
            _networkService.UpdateScoreTables();
        }

        [ClientRpc]
        public void RpcUpdateScoreTable(List<PlayerProfile> profiles)
        {
            if (!isOwned)
                return;
            Debug.Log(profiles.Count);
            _playerHUD.UpdateScoreTable(profiles);
        }

        private void Update()
        {
            if (isClient && isLocalPlayer)
                _playerStateMachine?.Tick();
        }

        [TargetRpc]
        public void TargetPlayerHit(uint hitSenderNetId)
        {
            if (_playerGameStatus.IsHitNow)
                return;
            _playerStateMachine.Enter<BasicMovementState>();
            _playerGameStatus.TargetPlayerHit();
            CmdSuccessHit(hitSenderNetId);
        }

        [Command]
        private void CmdSuccessHit(uint hitSenderNetId)
        {
            PlayerHitSuccessMessage message = new PlayerHitSuccessMessage()
            {
                HitSenderNetId = hitSenderNetId
            };
            NetworkServer.SendToAll(message);
        }

        [TargetRpc]
        public void TargetGameEnd()
        {
            _playerStateMachine.Enter<EndGameState>();
        }

        private void DisconnectFromGame()
        {
            if (isServer)
                NetworkServer.Shutdown();
            else
                NetworkClient.Disconnect();

            _gameStateMachine.Enter<MainMenuState>();
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
            _playerHUD.OnDisconnectButtonPressed -= DisconnectFromGame;
            _playerStateMachine.CleanUp();
            _playerInput.CleanUp();
        }
    }
}
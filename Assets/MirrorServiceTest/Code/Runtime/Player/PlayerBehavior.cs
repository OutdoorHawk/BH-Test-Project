using System.Collections.Generic;
using Mirror;
using MirrorServiceTest.Code.Infrastructure.Data;
using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Infrastructure.Services.Network;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService;
using MirrorServiceTest.Code.Infrastructure.Services.UI;
using MirrorServiceTest.Code.Infrastructure.StateMachine;
using MirrorServiceTest.Code.Infrastructure.StateMachine.States;
using MirrorServiceTest.Code.Runtime.Animation;
using MirrorServiceTest.Code.Runtime.CameraLogic;
using MirrorServiceTest.Code.Runtime.Player.Input;
using MirrorServiceTest.Code.Runtime.Player.StateMachine;
using MirrorServiceTest.Code.Runtime.Player.StateMachine.States;
using MirrorServiceTest.Code.Runtime.Player.Systems;
using MirrorServiceTest.Code.Runtime.Player.UI;
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
        private PlayerMovement _playerMovement;
        private PlayerAnimator _animator;
        private PlayerCollisionDetector _collisionDetector;
        private PlayerGameStatus _playerGameStatus;
        private IPlayerStateMachine _playerStateMachine;
        private PlayerHUD _playerHUD;
        private IUIFactory _uiFactory;
        private IGameNetworkService _networkService;
        private IGameStateMachine _gameStateMachine;
        private IRecordingService _recordingService;
        private WorldStaticData _worldStaticData;

        [ClientRpc]
        public void RpcConstruct(PlayerStaticData staticData, WorldStaticData worldStaticData)
        {
            _worldStaticData = worldStaticData;
            _playerStaticData = staticData;
            _uiFactory = DIContainer.Container.Resolve<IUIFactory>();
            _networkService = DIContainer.Container.Resolve<IGameNetworkService>();
            _gameStateMachine = DIContainer.Container.Resolve<IGameStateMachine>();
            _recordingService = DIContainer.Container.Resolve<IRecordingService>();
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
            _playerInput = new PlayerInput();
            _animator = new PlayerAnimator(animator);
            _cameraFollow = Instantiate(_cameraFollowPrefab);
            _playerMovement =
                new PlayerMovement(_playerStaticData, rigidbody, transform, _cameraFollow, this);
            _playerGameStatus = new PlayerGameStatus(_playerStaticData, this, changeComponent);
            _playerStateMachine =
                new PlayerStateMachine(_playerMovement, _playerInput, _animator, _collisionDetector,
                    _playerGameStatus, this, _playerStaticData);
        }

        private void InitSystems()
        {
            _playerInput.Init();
            _playerInput.EnableAllInput();
            _cameraFollow.Init(_playerInput, _playerStaticData, transform);
            _playerStateMachine.Enter<BasicMovementState>();
            _playerHUD.Init(_worldStaticData.GameRestartDelay, _playerInput, _recordingService);
            _playerGameStatus.OnPlayerHit += CmdAskForPlayerHit;
            _playerHUD.OnDisconnectButtonPressed += DisconnectFromGame;
            _recordingService.SetPlayerRecording(this);
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

        private void Update()
        {
            if (!isOwned)
                return;

            _playerStateMachine?.Tick();
            _playerStateMachine?.FixedTick();
        }

        public void CmdSetPlayerPosition(FrameData frameData)
        {
            GetComponent<NetworkTransform>().CmdTeleport(frameData.Position);
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

            _playerStateMachine.Enter<BasicMovementState>();
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
            _playerStateMachine.Enter<EndGameState>();
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
            _playerHUD.OnDisconnectButtonPressed -= DisconnectFromGame;
            _playerGameStatus.OnPlayerHit -= CmdAskForPlayerHit;
            _playerStateMachine.CleanUp();
            _playerInput.CleanUp();
        }
    }
}
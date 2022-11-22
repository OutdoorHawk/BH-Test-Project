using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.CameraLogic;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.Movement;
using BH_Test_Project.Code.Runtime.Player.StateMachine;
using BH_Test_Project.Code.Runtime.Player.StateMachine.States;
using BH_Test_Project.Code.Runtime.Player.UI;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerCollisionDetector))]
    [RequireComponent(typeof(ColorChangeComponent))]
    public class PlayerBehavior : NetworkBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private CameraFollow _cameraFollowPrefab;

        private CameraFollow _cameraFollow;
        private PlayerInput _playerInput;
        private PlayerMovement _playerMovement;
        private PlayerAnimator _animator;
        private PlayerCollisionDetector _collisionDetector;
        private PlayerGameStatus _playerGameStatus;
        private IPlayerStateMachine _playerStateMachine;
        private PlayerGameUI _playerGameUI;
        private NetworkPlayerSystem _playerGameSystem;

        private bool _playerIsHitNow => _playerStateMachine.ActiveState is HitState;

        private void Start()
        {
            if (isOwned)
                Init();
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            CmdAddNewPlayerToScoreTable(netId, PlayerPrefs.GetString(Constants.PLAYER_NAME));
        }

        [Command]
        private void CmdAddNewPlayerToScoreTable(uint netID, string playerName)
        {
            PlayerConnectedMessage playerConnectedMessage = new PlayerConnectedMessage()
            {
                NetId = netID,
                PlayerName = $"{playerName}{netID}"
            };
            NetworkServer.SendToAll(playerConnectedMessage);
        }

        private void Init()
        {
            CreateSystems();
            InitSystems();
            _playerInput.EnableAllInput();
            _playerInput.OnEscapePressed += ChangeCursorSettings;
        }

        private void CreateSystems()
        {
            Animator animator = GetComponent<Animator>();
            CharacterController characterController = GetComponent<CharacterController>();
            ColorChangeComponent changeComponent = GetComponent<ColorChangeComponent>();
            _playerGameUI = DIContainer.Container.Resolve<ISceneContextService>().GetPlayerUI();
            _playerGameSystem = DIContainer.Container.Resolve<ISceneContextService>().GetPlayerSystem();
            _collisionDetector = GetComponent<PlayerCollisionDetector>();
            _playerInput = new PlayerInput();
            _animator = new PlayerAnimator(animator);
            _cameraFollow = Instantiate(_cameraFollowPrefab);
            _playerMovement = new PlayerMovement(_playerData, characterController, transform, _cameraFollow, this);
            _playerGameStatus = new PlayerGameStatus(_playerData, this, changeComponent);
            _playerStateMachine =
                new PlayerStateMachine(_playerMovement, _playerInput, _animator, _collisionDetector,
                    netId, _playerGameStatus);
        }

        private void InitSystems()
        {
            _playerInput.Init();
            _cameraFollow.Init(_playerInput, _playerData, transform);
            _playerStateMachine.Enter<BasicMovementState>();
            _collisionDetector.Init(_playerData.PlayerCollisionMask);
        }

        private void Update()
        {
            if (isClient && isLocalPlayer)
                _playerStateMachine.Tick();
        }

        [TargetRpc]
        public void RpcHitPlayer(uint hitSenderNetId)
        {
            if (_playerIsHitNow)
                return;
            _playerStateMachine.Enter<HitState>();
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
        public void RpcIncreasePlayerScore(uint successPlayerNetId, int newScore)
        {
            _playerGameUI.UpdatePlayerScore(successPlayerNetId, newScore);
        }

        [TargetRpc]
        public void RpcGameEnd()
        {
            _playerStateMachine.Enter<EndGameState>();
        }

        private void ChangeCursorSettings()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                _playerInput.EnableAllInput();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                _playerInput.DisableAllInput();
            }
        }

        public override void OnStopLocalPlayer()
        {
            base.OnStopLocalPlayer();
            DisposePlayer();
        }

        private void DisposePlayer()
        {
            _playerStateMachine.CleanUp();
            _playerInput.OnEscapePressed -= ChangeCursorSettings;
        }
    }
}
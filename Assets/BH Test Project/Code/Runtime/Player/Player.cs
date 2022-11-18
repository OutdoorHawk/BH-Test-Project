using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.CameraLogic;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.Movement;
using BH_Test_Project.Code.Runtime.Player.StateMachine;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class Player : NetworkBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private CameraFollow _cameraFollowPrefab;

        private CameraFollow _cameraFollow;
        private PlayerInput _playerInput;
        private PlayerMovement _playerMovement;
        private PlayerAnimator _animator;
        private IPlayerStateMachine _playerStateMachine;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            if (isClient && isLocalPlayer)
                CreateSystems();
            InitSystems();
            _playerInput.EnableAllInput();
        }

        private void CreateSystems()
        {
            Animator animator = GetComponent<Animator>();
            CharacterController characterController = GetComponent<CharacterController>();
            _playerInput = new PlayerInput();
            _animator = new PlayerAnimator(animator);
            _cameraFollow = Instantiate(_cameraFollowPrefab);
            _playerMovement = new PlayerMovement(_playerInput, _playerData,
                characterController, transform, _animator, _cameraFollow, this);
            _playerStateMachine = new PlayerStateMachine(_playerInput);
        }

        private void InitSystems()
        {
            _playerInput.Init();
            _cameraFollow.Init(_playerInput, _playerData, transform);
        }

        private void Update()
        {
            _playerMovement.Tick();
            _playerStateMachine.Tick();
        }

        private void OnDestroy()
        {
            DisposeSystems();
        }

        private void DisposeSystems()
        {
            Destroy(_cameraFollow);
            _playerMovement.Cleanup();
        }
    }
}
using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.CameraLogic;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.Movement;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private CameraFollow _cameraFollow;

        private PlayerInput _playerInput;
        private PlayerMovement _playerMovement;
        private PlayerAnimator _animator;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            CreateSystems();
            InitSystems();
            _playerInput.EnableInput();
        }

        private void CreateSystems()
        {
            _playerInput = new PlayerInput();
            _animator = new PlayerAnimator(GetComponent<Animator>());
            _playerMovement = new PlayerMovement(_playerInput, _playerData,
                GetComponent<CharacterController>(), transform, _animator);
        }

        private void InitSystems()
        {
            _playerInput.Init();
            _cameraFollow.Init(_playerInput, _playerData, transform);
        }

        private void Update()
        {
            _playerMovement.Tick();
        }
        
    }
}
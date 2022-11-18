using BH_Test_Project.Code.Runtime.CameraLogic;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.Movement;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private CameraFollow _cameraFollow;
        private PlayerInput _playerInput;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _playerInput = new PlayerInput();
            _playerMovement = new PlayerMovement();
            InitSystems();
            _playerInput.EnableInput();
        }

        private void InitSystems()
        {
            _playerInput.Init();
            _playerMovement.Init(_playerInput, _playerData, GetComponent<CharacterController>(), transform);
            _cameraFollow.Init(_playerInput, _playerData, transform);
        }

        private void Update()
        {
            _playerMovement.Tick();
        }

        private void FixedUpdate()
        {
            _playerMovement.FixedTick();
        }
    }
}
using System;
using BH_Test_Project.Code.Player.Input;
using UnityEngine;

namespace BH_Test_Project.Code.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
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
        }

        private void Update()
        {
            _playerMovement.Tick();
        }
    }
}
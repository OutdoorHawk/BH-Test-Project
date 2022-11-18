using BH_Test_Project.Code.Player.Input;
using UnityEngine;

namespace BH_Test_Project.Code.Player
{
    public class PlayerMovement
    {
        private IPlayerInput _playerInput;
        private CharacterController _characterController;
        private Transform _cameraTransform;
        private Transform _playerTransform;

        private PlayerData _playerData;
        private Vector3 _movementVector;
        private Vector2 _inputVector;

        private const float MIN_MOVE_VALUE = 0.001f;

        public void Init(IPlayerInput playerInput, PlayerData playerData, CharacterController characterController,
            Transform playerTransform)
        {
            _playerTransform = playerTransform;
            _characterController = characterController;
            _playerInput = playerInput;
            _playerData = playerData;
            _cameraTransform = Camera.main?.transform;
        }

        public void Tick()
        {
            _inputVector = _playerInput.Movement.ReadValue<Vector2>();

            if (_inputVector.sqrMagnitude > MIN_MOVE_VALUE)
            {
                _movementVector = _cameraTransform.TransformDirection(_inputVector);
                _movementVector.y = 0;
                _movementVector.Normalize();
                _playerTransform.forward = _movementVector;
                
                _movementVector += Physics.gravity;
                _characterController.Move(_movementVector * (Time.deltaTime * _playerData.MovementSpeed));
            }

        }
    }
}
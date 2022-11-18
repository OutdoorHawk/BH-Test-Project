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
        private Vector3 _inputVector;
        private Vector3 _movementVector;

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
            ReadCurrentInput();

            if (!InputMoreThanMinValue()) 
                return;
            
            CalculateMovementVector();
            ApplyMovement();
        }

        private void ReadCurrentInput()
        {
            Vector2 input = _playerInput.Movement.ReadValue<Vector2>();
            _inputVector.Set(input.x, 0, input.y);
        }

        private bool InputMoreThanMinValue() => 
            _inputVector.sqrMagnitude > MIN_MOVE_VALUE;

        private void CalculateMovementVector()
        {
            _movementVector = _cameraTransform.TransformDirection(_inputVector);
            _movementVector.Normalize();
            _movementVector.y = 0;

            if (_movementVector != Vector3.zero)
                _playerTransform.forward = _movementVector;

            _movementVector += Physics.gravity;
        }

        private void ApplyMovement() => 
            _characterController.Move(_movementVector * (Time.deltaTime * _playerData.MovementSpeed));
    }
}
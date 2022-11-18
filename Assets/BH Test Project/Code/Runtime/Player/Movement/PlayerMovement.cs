using System.Collections;
using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.CameraLogic;
using BH_Test_Project.Code.Runtime.Player.Input;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player.Movement
{
    public class PlayerMovement
    {
        private readonly IPlayerInput _playerInput;
        private readonly CharacterController _characterController;
        private readonly Transform _cameraTransform;
        private readonly Transform _playerPlayerTransform;
        private readonly PlayerAnimator _playerAnimator;
        private readonly MonoBehaviour _mono;

        private readonly PlayerData _playerData;
        private Vector3 _inputVector;
        private Vector3 _movementVector;

        public PlayerMovement(PlayerInput playerInput, PlayerData playerData,
            CharacterController characterController, Transform playerTransform, PlayerAnimator playerAnimator,
            CameraFollow cameraFollow, MonoBehaviour mono)
        {
            _mono = mono;
            _playerPlayerTransform = playerTransform;
            _playerAnimator = playerAnimator;
            _characterController = characterController;
            _playerInput = playerInput;
            _playerData = playerData;
            _cameraTransform = cameraFollow.transform;
            _playerInput.OnDashPressed += PerformDash;
        }

        private const float MIN_MOVE_VALUE = 0.01f;
        private const float SMOOTH_TIME = 0.075f;
        private const float LERP_RATE = 50f;

        public void Tick()
        {
            ReadCurrentInput();
            SetPlayerSpeedToAnimator();
            CalculateMovementVector();
            ApplyMovement();
        }

        private void SetPlayerSpeedToAnimator()
        {
            Vector3 playerVelocity =
                new Vector3(_characterController.velocity.x, 0, _characterController.velocity.y);
            _playerAnimator.SetPlayerSpeed(playerVelocity.normalized.magnitude);
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
            if (InputMoreThanMinValue())
                ApplyToCurrentVector();
            else
                LerpToNewMovementVector(Vector3.zero);

            _movementVector += Physics.gravity;
        }

        private void ApplyToCurrentVector()
        {
            Vector3 nextMovementVector = _cameraTransform.TransformDirection(_inputVector);
            nextMovementVector.Normalize();

            LerpToNewMovementVector(nextMovementVector);
            _movementVector.y = 0;

            if (nextMovementVector != Vector3.zero)
                _playerPlayerTransform.forward = _movementVector;
        }

        private void LerpToNewMovementVector(Vector3 nextVector)
        {
            _movementVector = Vector3.Lerp(_movementVector, nextVector, Time.deltaTime * LERP_RATE);
        }

        private void ApplyMovement() =>
            _characterController.Move(_movementVector * (Time.deltaTime * _playerData.MovementSpeed));

        private void PerformDash()
        {
            _mono.StartCoroutine(Dashing());
        }

        private IEnumerator Dashing()
        {
            Vector3 dashVector = _movementVector;
            _playerInput.DisableAllInput();
            float t = _playerData.DashTime;
            do
            {
                t -= Time.deltaTime;
                _characterController.Move(dashVector * (Time.deltaTime * _playerData.DashPower));
                yield return new WaitForSeconds(Time.deltaTime);
            } while (t > 0);
            
            _playerInput.EnableAllInput();
        }

        public void Cleanup()
        {
            _playerInput.OnDashPressed -= PerformDash;
        }
    }
}
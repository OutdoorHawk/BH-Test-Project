using System;
using System.Collections;
using BH_Test_Project.Code.Runtime.CameraLogic;
using BH_Test_Project.Code.StaticData;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player.Movement
{
    public class PlayerMovement
    {
        public event Action OnDashEnded;
        
        private readonly CharacterController _characterController;
        private readonly Transform _cameraTransform;
        private readonly Transform _playerPlayerTransform;
        private readonly MonoBehaviour _mono;

        private readonly PlayerStaticData _playerStaticData;
        private Vector3 _inputVector;
        private Vector3 _movementVector;

        public PlayerMovement(PlayerStaticData playerStaticData, CharacterController characterController,
            Transform playerTransform, CameraFollow cameraFollow, MonoBehaviour mono)
        {
            _mono = mono;
            _playerPlayerTransform = playerTransform;
            _characterController = characterController;
            _playerStaticData = playerStaticData;
            _cameraTransform = cameraFollow.transform;
        }

        private const float MIN_MOVE_VALUE = 0.01f;
        private const float SMOOTH_TIME = 0.075f;
        private const float LERP_RATE = 50f;

        public void UpdateInput(Vector2 movementInput)
        {
            ReadCurrentInput(movementInput);
            CalculateMovementVector();
        }

        public void Tick()
        {
            ApplyMovement();
        }

        public float GetPlayerSpeed()
        {
            return new Vector3(_characterController.velocity.x, 0, _characterController.velocity.y)
                .normalized.magnitude;
        }

        private void ReadCurrentInput(Vector2 input)
        {
            _inputVector.Set(input.x, 0, input.y);
        }

        private void CalculateMovementVector()
        {
            if (InputMoreThanMinValue())
                ApplyToCurrentMovementVector();
            else
                LerpToNewMovementVector(Vector3.zero);

            _movementVector += Physics.gravity;
        }

        private bool InputMoreThanMinValue() =>
            _inputVector.sqrMagnitude > MIN_MOVE_VALUE;

        private void ApplyToCurrentMovementVector()
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
            _characterController.Move(_movementVector * (Time.deltaTime * _playerStaticData.MovementSpeed));

        public void PerformDash()
        {
            _movementVector = Vector3.zero;
            _mono.StartCoroutine(Dashing());
        }

        private IEnumerator Dashing()
        {
            Vector3 dashVector = _playerPlayerTransform.forward * _playerStaticData.DashPower;
            float t = _playerStaticData.DashTime;
            do
            {
                t -= Time.deltaTime;
                LerpToNewMovementVector(dashVector);
                yield return new WaitForSeconds(Time.deltaTime);
            } while (t > 0);

            OnDashEnded?.Invoke();
        }
    }
}
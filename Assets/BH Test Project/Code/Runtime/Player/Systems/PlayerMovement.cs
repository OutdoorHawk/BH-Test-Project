using System;
using System.Collections;
using BH_Test_Project.Code.Runtime.CameraLogic;
using BH_Test_Project.Code.StaticData;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player.Systems
{
    public class PlayerMovement
    {
        public event Action OnDashEnded;

        private readonly CharacterController _characterController;
        private readonly Transform _cameraTransform;
        private readonly Transform _playerTransform;
        private readonly MonoBehaviour _mono;
        private readonly PlayerStaticData _playerStaticData;

        private IEnumerator _dashRoutine;
        private Vector3 _inputVector;
        private Vector3 _movementVector;
        private Vector3 _velocity = Vector3.zero;

        public PlayerMovement(PlayerStaticData playerStaticData, CharacterController characterController,
            Transform playerTransform, CameraFollow cameraFollow, MonoBehaviour mono)
        {
            _mono = mono;
            _playerTransform = playerTransform;
            _characterController = characterController;
            _playerStaticData = playerStaticData;
            _cameraTransform = cameraFollow.transform;
        }

        private const float MIN_MOVE_VALUE = 0.01f;
        private const float LERP_RATE = 35f;
        private const float FORWARD_LERP_RATE = 0.4f;

        public void UpdateInput(Vector2 movementInput)
        {
            ReadCurrentInput(movementInput);
            CalculateMovementVector();
        }

        public void Tick()
        {
            ApplyMovement();
        }

        public float GetNormalizedPlayerSpeed()
        {
            Vector3 velocity = _characterController.velocity;
            return new Vector3(velocity.x, 0, velocity.z).normalized.magnitude;
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
            Vector3 transformedVector = _cameraTransform.TransformDirection(_inputVector);
            Vector3 nextMovementVector = new Vector3(transformedVector.x, 0, transformedVector.z);
            nextMovementVector.Normalize();

            LerpToNewMovementVector(nextMovementVector);
            _movementVector.y = 0;

            if (nextMovementVector != Vector3.zero)
                ApplyForwardRotation(nextMovementVector);
        }

        private void LerpToNewMovementVector(Vector3 nextVector)
        {
            _movementVector = Vector3.Lerp(_movementVector, nextVector, Time.deltaTime * LERP_RATE);
        }

        private void ApplyForwardRotation(Vector3 nextMovementVector)
        {
            _playerTransform.forward = Vector3.Lerp(_playerTransform.forward, nextMovementVector, FORWARD_LERP_RATE);
        }

        private void ApplyMovement()
        {
            _characterController.Move(_movementVector * (Time.deltaTime * _playerStaticData.MovementSpeed));
        }

        public void PerformDash()
        {
            _movementVector = Vector3.zero;
            _dashRoutine = Dashing();
            _mono.StartCoroutine(_dashRoutine);
        }

        private IEnumerator Dashing()
        {
            Vector3 dashVector = _playerTransform.forward * _playerStaticData.DashDistance;
            float distance = dashVector.magnitude;

            while (distance > 0)
            {
                distance -= _playerStaticData.MovementSpeed * Time.deltaTime;
                LerpToNewMovementVector(dashVector);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            _dashRoutine = null;
            OnDashEnded?.Invoke();
        }

        public void StopDash()
        {
            if (_dashRoutine != null) 
                _mono.StopCoroutine(_dashRoutine);
        }
    }
}
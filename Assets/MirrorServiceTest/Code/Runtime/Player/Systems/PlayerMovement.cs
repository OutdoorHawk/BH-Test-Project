using System;
using System.Collections;
using MirrorServiceTest.Code.Runtime.CameraLogic;
using MirrorServiceTest.Code.StaticData;
using UnityEngine;

namespace MirrorServiceTest.Code.Runtime.Player.Systems
{
    public class PlayerMovement
    {
        public event Action OnDashEnded;

        private readonly Rigidbody _rigidbody;
        private readonly Transform _cameraTransform;
        private readonly Transform _playerTransform;
        private readonly MonoBehaviour _mono;
        private readonly PlayerStaticData _playerStaticData;

        private IEnumerator _dashRoutine;
        private Vector3 _inputVector;
        private Vector3 _movementVector;

        public PlayerMovement(PlayerStaticData playerStaticData, Rigidbody rigidbody,
            Transform playerTransform, CameraFollow cameraFollow, MonoBehaviour mono)
        {
            _mono = mono;
            _playerTransform = playerTransform;
            _rigidbody = rigidbody;
            _playerStaticData = playerStaticData;
            _cameraTransform = cameraFollow.transform;
        }

        private const float MIN_MOVE_VALUE = 0.01f;
        private const float LERP_RATE = 35f;
        private const float FORWARD_LERP_RATE = 0.4f;

        public void UpdateInput(Vector2 movementInput)
        {
            ReadCurrentInput(movementInput);
        }

        public float GetNormalizedPlayerSpeed()
        {
            Vector3 velocity = _rigidbody.velocity;
            return new Vector3(velocity.x, 0, velocity.z).normalized.magnitude;
        }

        public void ApplyMovement()
        {
            _rigidbody.velocity = _movementVector * _playerStaticData.MovementSpeed;
        }

        public void PerformDash()
        {
            _movementVector = Vector3.zero;
            _dashRoutine = Dashing();
            _mono.StartCoroutine(_dashRoutine);
        }

        private void ReadCurrentInput(Vector2 input)
        {
            _inputVector.Set(input.x, 0, input.y);
        }

        public void CalculateMovementVector()
        {
            if (InputMoreThanMinValue())
                TransformAndUpdateCurrentVector();
            else
                //LerpToNewMovementVector(Vector3.zero);
                _movementVector = Vector3.zero;
        }

        private bool InputMoreThanMinValue() =>
            _inputVector.sqrMagnitude > MIN_MOVE_VALUE;

        private void TransformAndUpdateCurrentVector()
        {
            Vector3 transformedVector = _cameraTransform.TransformDirection(_inputVector);
            Vector3 nextMovementVector = new Vector3(transformedVector.x, 0, transformedVector.z);
            nextMovementVector.Normalize();

            _movementVector = nextMovementVector;

            if (nextMovementVector != Vector3.zero)
                ApplyForwardRotation(nextMovementVector);
        }

        private void LerpToNewMovementVector(Vector3 nextVector)
        {
            _movementVector = Vector3.Lerp(_movementVector, nextVector, LERP_RATE);
        }

        private void ApplyForwardRotation(Vector3 nextMovementVector)
        {
            _playerTransform.forward = Vector3.Lerp(_playerTransform.forward, nextMovementVector, FORWARD_LERP_RATE);
        }

        private IEnumerator Dashing()
        {
            Vector3 dashVector = _playerTransform.forward * _playerStaticData.DashDistance;
            _rigidbody.useGravity = false;
            float distance = dashVector.magnitude;

            while (distance > 0)
            {
                distance -= _playerStaticData.MovementSpeed * Time.fixedDeltaTime;
                _movementVector = dashVector.normalized;
                yield return new WaitForFixedUpdate();
            }

            _rigidbody.useGravity = true;
            _dashRoutine = null;
            OnDashEnded?.Invoke();
        }

        public void StopDash()
        {
            if (_dashRoutine != null)
                _mono.StopCoroutine(_dashRoutine);
            _rigidbody.useGravity = true;
        }
    }
}
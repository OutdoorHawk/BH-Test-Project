using BH_Test_Project.Code.Player.Input;
using UnityEngine;

namespace BH_Test_Project.Code.Player.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _followTarget;
        [SerializeField] private float _cameraHeight;
        [SerializeField] private float _cameraDistance;
        [SerializeField] private float _smoothTime = 3;
        [SerializeField] private float _lerpRate = 10f;
        [SerializeField] private Vector2 _yClamp;

        private Transform _cachedTransform;
        private IPlayerInput _playerInput;
        private Vector3 _currentPosition;
        private Vector3 _currentRotation;
        private PlayerData _playerData;
        private Vector2 _mouseAxis;
        private Vector3 _smoothVelocity = Vector3.zero;

        private float xRotation;
        private float yRotation;

        public void Init(IPlayerInput playerInput, PlayerData playerData, Transform target)
        {
            _playerData = playerData;
            _playerInput = playerInput;
            _followTarget = target;
            _cachedTransform = transform;
        }

        private void LateUpdate()
        {
            if (_followTarget == null)
                return;

            CalculateCameraPosition();
            CalculateCameraRotation();
            ApplyCameraTransformValues();
        }

        private void CalculateCameraPosition()
        {
            Vector3 targetPosition = _followTarget.position;
            Vector3 resultPosition = new Vector3(targetPosition.x,
                targetPosition.y + _cameraHeight, targetPosition.z) - _cachedTransform.forward * _cameraDistance;
            _currentPosition = Vector3.Lerp(_currentPosition, resultPosition, Time.deltaTime * _lerpRate);
        }

        private void CalculateCameraRotation()
        {
            _mouseAxis = _playerInput.MouseAxis.ReadValue<Vector2>();

            float mouseX = _mouseAxis.x;
            float mouseY = _mouseAxis.y;

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, _yClamp.x, _yClamp.y);

            Vector3 nextRotation = new Vector3(xRotation, yRotation);
            _currentRotation =
                Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        }

        private void ApplyCameraTransformValues()
        {
            _cachedTransform.localEulerAngles = _currentRotation;
            _cachedTransform.localPosition = _currentPosition;
        }
    }
}
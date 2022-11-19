using BH_Test_Project.Code.Runtime.Player;
using BH_Test_Project.Code.Runtime.Player.Input;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _followTarget;
        [SerializeField] private float _cameraHeight;
        [SerializeField] private float _cameraDistance;
        [SerializeField] private float _smoothTime = 3;
        [SerializeField] private float _lerpRate = 10f;
        [SerializeField] private Vector2 _yClamp;
        [SerializeField] private LayerMask _obstructionMask;

        private Transform _cachedTransform;
        private IPlayerInput _playerInput;
        private Camera _camera;
        private Vector3 _currentPosition;
        private Vector3 _currentRotation;
        private PlayerData _playerData;
        private Vector2 _mouseAxis;
        private Vector3 _smoothVelocity = Vector3.zero;
        private Vector3 _focusPoint;

        private float xRotation;
        private float yRotation;

        private const float RAYCAST_MAX_DISTANCE = 5;

        Vector3 CameraHalfExtends
        {
            get
            {
                Vector3 halfExtends;
                halfExtends.y =
                    _camera.nearClipPlane *
                    Mathf.Tan(0.5f * Mathf.Deg2Rad * _camera.fieldOfView);
                halfExtends.x = halfExtends.y * _camera.aspect;
                halfExtends.z = 0f;
                return halfExtends;
            }
        }

        public void Init(IPlayerInput playerInput, PlayerData playerData, Transform target)
        {
            _playerData = playerData;
            _playerInput = playerInput;
            _followTarget = target;
            _cachedTransform = transform;
            _camera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            if (_followTarget == null)
            {
                Destroy(gameObject);
                return;
            }

            CalculateCameraPosition();
            CalculateCameraRotation();
            ApplyCameraTransformValues();
          // CheckCameraCollision();
        }

        private void CheckCameraCollision()
        {
            Quaternion lookRotation = _cachedTransform.localRotation;
            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = _focusPoint - lookDirection * _cameraDistance;

            Vector3 rectOffset = lookDirection * _camera.nearClipPlane;
            Vector3 rectPosition = lookPosition + rectOffset;
            Vector3 castFrom = _focusPoint;
            Vector3 castLine = rectPosition - castFrom;
            float castDistance = castLine.magnitude;
            Vector3 castDirection = castLine / castDistance;

            if (Physics.BoxCast(castFrom, CameraHalfExtends, castDirection, out RaycastHit hit,
                    lookRotation, castDistance, _obstructionMask))
            {
                rectPosition = castFrom + castDirection * hit.distance;
                lookPosition = rectPosition - rectOffset;
            }

            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

        private void CalculateCameraPosition()
        {
            Vector3 targetPosition = _followTarget.position;
            _focusPoint = new Vector3(targetPosition.x,
                targetPosition.y + _cameraHeight, targetPosition.z) - _cachedTransform.forward * _cameraDistance;
            _currentPosition = Vector3.Lerp(_currentPosition, _focusPoint, Time.deltaTime * _lerpRate);
        }

        private void CalculateCameraRotation()
        {
            _mouseAxis = _playerInput.MouseAxis.ReadValue<Vector2>();

            float mouseX = _mouseAxis.x * _playerData.MouseSensitivity;
            float mouseY = _mouseAxis.y * _playerData.MouseSensitivity;

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
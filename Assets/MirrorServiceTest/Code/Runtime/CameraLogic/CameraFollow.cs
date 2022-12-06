using MirrorServiceTest.Code.Runtime.Player.Input;
using MirrorServiceTest.Code.StaticData;
using UnityEngine;

namespace MirrorServiceTest.Code.Runtime.CameraLogic
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _followTarget;
        [SerializeField] private float _cameraHeight;
        [SerializeField] private float _cameraDistance;
        [SerializeField] private float _smoothTime = 3;
        [SerializeField] private float _lerpRate = 10f;
        [SerializeField] private Vector2 _yClamp;
        [SerializeField] private LayerMask _collisionMask;

        private Collider[] _colliders;
        private Transform _cachedTransform;
        private IPlayerInput _playerInput;
        private Vector3 _currentPosition;
        private Vector3 _currentRotation;
        private PlayerStaticData _playerStaticData;
        private Vector2 _mouseAxis;
        private Vector3 _smoothVelocity = Vector3.zero;
        private Vector3 _focusPoint;

        private float _defaultCameraDistance;
        private float _xRotation;
        private float _yRotation;

        private const float COLLISION_RADIUS = 0.75f;
        private const float CAMERA_MIN_DISTANCE = 0.85f;

        private void Awake()
        {
            _colliders = new Collider[1];
            _defaultCameraDistance = _cameraDistance;
        }

        public void Init(IPlayerInput playerInput, PlayerStaticData playerStaticData, Transform target)
        {
            _playerStaticData = playerStaticData;
            _playerInput = playerInput;
            _followTarget = target;
            _cachedTransform = transform;
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
            CheckCameraCollision();
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

            float mouseX = _mouseAxis.x * _playerStaticData.MouseSensitivity;
            float mouseY = _mouseAxis.y * _playerStaticData.MouseSensitivity;

            _yRotation += mouseX;
            _xRotation -= mouseY;

            _xRotation = Mathf.Clamp(_xRotation, _yClamp.x, _yClamp.y);

            Vector3 nextRotation = new Vector3(_xRotation, _yRotation);
            _currentRotation =
                Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        }

        private void ApplyCameraTransformValues()
        {
            _cachedTransform.localEulerAngles = _currentRotation;
            _cachedTransform.localPosition = _currentPosition;
        }

        private void CheckCameraCollision()
        {
            Physics.OverlapSphereNonAlloc(_cachedTransform.position, COLLISION_RADIUS, _colliders, _collisionMask);

            if (_colliders[0] != null)
            {
                _cameraDistance =
                    Mathf.Clamp(Vector3.Distance(_colliders[0].transform.position, _cachedTransform.position),
                        CAMERA_MIN_DISTANCE, _defaultCameraDistance);
            }
            else
                _cameraDistance = _defaultCameraDistance;
        }
    }
}
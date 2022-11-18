using BH_Test_Project.Code.Player.Input;
using UnityEngine;

namespace BH_Test_Project.Code.Player.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _followTarget;
        [SerializeField] private Vector3 _cameraOffset;
        [SerializeField] private float _smoothFactor;
        [SerializeField] private Vector2 _yClamp;
        [SerializeField] private float _lookOffset;

        private Transform _cachedTransform;
        private IPlayerInput _playerInput;
        private Vector3 _cameraPosition;
        private Quaternion _cameraRotation;
        private PlayerData _playerData;
        private Vector2 _mouseAxis;

        private float yRotation;
        private float xRotation;

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
            ApplyCameraPosition();
        }

        private void CalculateCameraPosition()
        {
            _mouseAxis = _playerInput.MouseAxis.ReadValue<Vector2>();
 
            float mouseX = _mouseAxis.x;
            float mouseY = _mouseAxis.y;

            _cameraRotation.y += mouseX;
            _cameraRotation.x -= mouseY;

            _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, _yClamp.x, _yClamp.y);
            _cameraPosition = _followTarget.position - _cachedTransform.forward * 3;
        }

        private void ApplyCameraPosition()
        {
            _cachedTransform.position = _cameraPosition;
            _cachedTransform.eulerAngles = new Vector3(_cameraRotation.x, _cameraRotation.y, 0);
        }
    }
}
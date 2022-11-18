using System;
using UnityEngine;

namespace BH_Test_Project.Code.Player.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _followTarget;
        [SerializeField] private Vector3 _cameraOffset;
        [SerializeField] private float _distance;

        [SerializeField] private float _rotationAngleX;
        [SerializeField] private float _rotationAngleY;
        [SerializeField] private float _rotationAngleZ;

        private Transform _cachedTransform;
        private Quaternion _rotation;
        private Vector3 _position;

        private void Awake()
        {
            _cachedTransform = transform;
        }

        public void FollowTarget(Transform target)
        {
            _followTarget = target;
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
            //_rotation = Quaternion.Euler(_rotationAngleX, _rotationAngleY, _rotationAngleZ);
            _position = _rotation * _cameraOffset + _followTarget.position;
        }

        private void ApplyCameraPosition()
        {
            _cachedTransform.position = _position;
            _cachedTransform.rotation = _rotation;
        }
    }
}
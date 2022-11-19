using System;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player
{
    public class PlayerCollisionDetector : MonoBehaviour
    {
        public event Action<ControllerColliderHit> OnPlayerCollided;
        
        private readonly CharacterController _characterController;
        private LayerMask _playerCollisionMask;

        public void Init(LayerMask playerCollisionMask)
        {
            _playerCollisionMask = playerCollisionMask;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (IsObjectInLayerMask(_playerCollisionMask, hit.gameObject.layer)) 
                OnPlayerCollided?.Invoke(hit);
        }

        private bool IsObjectInLayerMask(LayerMask mask, int layer)
        {
            return (mask.value & (1 << layer)) > 0;
        }
    }
}
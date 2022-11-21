using System;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player
{
    [Serializable]
    public struct PlayerData
    {
        public float MovementSpeed;
        public float MouseSensitivity;
        public float DashPower; //TODO Change to distance
        public float DashTime;
        public float HitTime;
        public LayerMask PlayerCollisionMask;
    }
}
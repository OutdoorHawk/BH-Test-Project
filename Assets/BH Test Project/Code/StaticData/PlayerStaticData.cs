using System;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.StaticData
{
    [Serializable]
    public struct PlayerStaticData
    {
        [Range(0.1f, 30)] public float MovementSpeed;
        [Range(0.1f, 10)] public float MouseSensitivity;
        [Range(0.1f, 99)] public float DashDistance;
        [Range(0.1f, 10)] public float DashRechargeTime;
        [Range(0.1f, 10)] public float HitTime;
    }
}
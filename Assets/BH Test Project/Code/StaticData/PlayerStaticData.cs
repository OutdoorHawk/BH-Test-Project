using System;
using UnityEngine;

namespace BH_Test_Project.Code.StaticData
{
    [Serializable]
    public struct PlayerStaticData
    {
        [Range(0.1f, 10)] public float MovementSpeed;
        [Range(0.1f, 10)] public float MouseSensitivity;
        [Range(0.1f, 99)] public float DashPower; //TODO Change to distance
        [Range(0.1f, 10)] public float DashTime;
        [Range(0.1f, 10)] public float HitTime;
    }
}
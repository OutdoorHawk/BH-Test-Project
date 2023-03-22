using System;
using Mirror;
using UnityEngine;

namespace MirrorServiceTest.Code.StaticData
{
    [Serializable]
    public struct PlayerStaticData
    {
        [Range(0.1f, 30),SyncVar] public float MovementSpeed;
        [Range(0.1f, 10),SyncVar] public float MouseSensitivity;
        [Range(0.1f, 99),SyncVar] public float DashDistance;
        [Range(0.1f, 10),SyncVar] public float DashRechargeTime;
        [Range(0.1f, 10),SyncVar] public float HitTime;
    }
}
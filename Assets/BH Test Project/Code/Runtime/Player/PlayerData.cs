using System;

namespace BH_Test_Project.Code.Runtime.Player
{
    [Serializable]
    public struct PlayerData
    {
        public float MovementSpeed;
        public float MouseSensitivity;
        public float DashPower;
        public float DashTime;
    }
}
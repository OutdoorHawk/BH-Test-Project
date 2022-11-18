using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network.Data
{
    public struct PositionMessage : NetworkMessage
    {
        public Vector3 Vector3;
    }
}
using BH_Test_Project.Code.Runtime.Player;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network.Data
{
    public struct SpawnPlayerMessage : NetworkMessage
    {
        public Vector3 SpawnPosition;
    }
}
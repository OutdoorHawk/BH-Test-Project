using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network.Data
{
    public struct CreatePlayerMessage : NetworkMessage
    {
        public Vector3 SpawnPosition;
        public int Id;
        public string Name;
    }
}
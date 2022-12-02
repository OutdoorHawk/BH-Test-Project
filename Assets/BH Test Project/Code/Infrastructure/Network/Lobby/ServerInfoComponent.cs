using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network.Lobby
{
    public class ServerInfoComponent : NetworkBehaviour
    {
        public SyncDictionary<int, GameObject> PlayersOnServer { get; } = new();
        
        public void AddPlayerToServerInfo(int connId, GameObject playerObject)
        {
            PlayersOnServer.Add(connId, playerObject);

            foreach (var VARIABLE in PlayersOnServer.Values)
            {
                Debug.Log(VARIABLE.name);
            }
        }

    }
}
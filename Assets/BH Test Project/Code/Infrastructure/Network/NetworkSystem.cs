using BH_Test_Project.Code.Infrastructure.Network.Data;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkSystem : NetworkManager
    {
        private NetworkConnection _connection;
        private bool _playerSpawned;
        private bool _playerConnected;

        public void OnCreateCharacter(NetworkConnectionToClient connection, SpawnPositionMessage spawnPosition)
        {
            GameObject go = Instantiate(playerPrefab, spawnPosition.Vector3, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(connection, go);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            //NetworkServer.RegisterHandler<SpawnPositionMessage>(OnCreateCharacter);
        }
        
        /*public override void OnClientConnect()
        {
            base.OnClientConnect();
            SpawnPositionMessage m = new SpawnPositionMessage() { Vector3 = _secondSpawn.position };
            _connection.Send(m);
        }*/
    }
}
using System.Collections.Generic;
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

        public void OnCreateCharacter(NetworkConnectionToClient connection, PositionMessage position)
        {
            GameObject go = Instantiate(playerPrefab, position.Vector3, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(connection, go);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<PositionMessage>(OnCreateCharacter);
        }


        public void ActivatePlayerSpawn()
        {
            Vector3 pos = Input.mousePosition;
            pos.z = 10f;
            pos = Camera.main.ScreenToWorldPoint(pos);

            PositionMessage m = new PositionMessage()
            {
                Vector3 = pos
            };
            
            _connection.Send(m);
            _playerSpawned = true;
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            
        }
    }
}
using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkSystem : NetworkManager
    {
        [SerializeField] private List<Transform> _spawnPoints;

        private NetworkSpawnSystem _spawnSystem;
        private NetworkPlayerSystem _playerSystem;
        private int _playerID;

        public override void OnStartServer()
        {
            base.OnStartServer();
            CreateSystems();
            NetworkServer.RegisterHandler<SpawnPlayerMessage>(OnCreateCharacter);
        }

        private void CreateSystems()
        {
            _spawnSystem = new NetworkSpawnSystem(playerPrefab, _spawnPoints);
            _playerSystem = new NetworkPlayerSystem();
        }


        public override void OnClientConnect()
        {
            base.OnClientConnect();
            SpawnNewPlayer();
        }

        private void SpawnNewPlayer()
        {
            SpawnPlayerMessage spawnPlayerMessage = new SpawnPlayerMessage
            {
                SpawnPosition = _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)].position
            };

            NetworkClient.Send(spawnPlayerMessage);
        }

        private void OnCreateCharacter(NetworkConnectionToClient conn, SpawnPlayerMessage message)
        {
            GameObject go = Instantiate(playerPrefab, message.SpawnPosition, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, go);
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            _playerSystem?.CheckPlayersLeft();
        }
    }
}
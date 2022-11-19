using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkSystem : NetworkManager
    {
        [SerializeField] private List<Transform> _spawnPoints;

        private NetworkSpawnSystem _spawnSystem;
        private NetworkPlayerSystem _playerSystem;

        public override void OnStartHost()
        {
            base.OnStartHost();
            CreateSystems();
            Subscribe();
            Debug.Log("OnStartHost");
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log("OnStartServer");
        }

        [Server]
        private void CreateSystems()
        {
            _spawnSystem = new NetworkSpawnSystem(playerPrefab, _spawnPoints);
            _playerSystem = new NetworkPlayerSystem();
            Debug.Log("createSystems");
        }

        [Server]
        private void Subscribe()
        {
            _spawnSystem.OnPlayerSpawned += _playerSystem.AddNewPlayerToList;
        }

        [Server]
        private void CleanUp()
        {
            _spawnSystem.OnPlayerSpawned -= _playerSystem.AddNewPlayerToList;
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            _spawnSystem.SpawnNewPlayer();
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            CleanUp();
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            _playerSystem?.CheckPlayersLeft();
        }
    }
}
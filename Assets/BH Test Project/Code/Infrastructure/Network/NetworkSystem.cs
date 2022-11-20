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
        private int _playerID;

        public override void Start()
        {
            base.Start();
            CreateSystems();
            Subscribe();
        }

        private void Subscribe()
        {
            _spawnSystem.OnPlayerSpawned += _playerSystem.AddNewPlayer;
        }

        private void CleanUp()
        {
            _spawnSystem.OnPlayerSpawned -= _playerSystem.AddNewPlayer;
        }

        private void CreateSystems()
        {
            _spawnSystem = new NetworkSpawnSystem(playerPrefab, _spawnPoints);
            _playerSystem = new NetworkPlayerSystem();
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            _spawnSystem.SpawnNewPlayer();
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            _playerSystem.RemovePlayer(conn);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            CleanUp();
        }
    }
}
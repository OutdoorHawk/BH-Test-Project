using System.Collections.Generic;
using BH_Test_Project.Code.Runtime.UI;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class GameLoopNetworkManager : NetworkManager
    {
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private PlayerGameUI _playerUI;

        private NetworkSpawnSystem _spawnSystem;
        private NetworkPlayerSystem _playerSystem;
        private int _playerID;

        public override void OnStartClient()
        {
            base.OnStartClient();
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
            _spawnSystem.RegisterHandlers();
            _playerSystem.RegisterHandlers();
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

        public override void OnStopServer()
        {
            base.OnStopServer();
            CleanUp();
        }
    }
}
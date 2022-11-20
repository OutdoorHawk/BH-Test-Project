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

        private void CreateSystems()
        {
            _spawnSystem = new NetworkSpawnSystem(playerPrefab, _spawnPoints);
            _playerSystem = new NetworkPlayerSystem();
        }

        private void Subscribe()
        {
            _spawnSystem.OnPlayerSpawned += _playerSystem.AddNewPlayerToList;
        }

        private void Unsubscribe()
        {
            _spawnSystem.OnPlayerSpawned -= _playerSystem.AddNewPlayerToList;
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            Debug.Log("ServerConnect");
           // _spawnSystem.SpawnNewPlayer(conn);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            _spawnSystem.SpawnNewPlayer(null);
        }


        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
           // conn.identity.
                
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Unsubscribe();
        }
        
    }
}
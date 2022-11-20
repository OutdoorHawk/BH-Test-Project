using System;
using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.Player;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkSpawnSystem
    {
        public event Action<Player,NetworkConnectionToClient> OnPlayerSpawned;

        private readonly GameObject _playerPrefab;
        private readonly List<Transform> _spawnPoints;

        private int _currentPlayerId;

        public NetworkSpawnSystem(GameObject playerPrefab, List<Transform> spawnPoints)
        {
            _playerPrefab = playerPrefab;
            _spawnPoints = spawnPoints;
            NetworkServer.RegisterHandler<SpawnPlayerMessage>(OnCreateCharacter);
        }
        
        public void SpawnNewPlayer()
        {
            SpawnPlayerMessage message = new SpawnPlayerMessage()
            {
                SpawnPosition = _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)].position,
                Id = ++_currentPlayerId
            };
            
            NetworkClient.Send(message);
        }

        private void OnCreateCharacter(NetworkConnectionToClient conn, SpawnPlayerMessage message)
        {
            GameObject go = Object.Instantiate(_playerPrefab, message.SpawnPosition, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, go);
            OnPlayerSpawned?.Invoke(go.GetComponent<Player>(), conn);
        }
    }
}
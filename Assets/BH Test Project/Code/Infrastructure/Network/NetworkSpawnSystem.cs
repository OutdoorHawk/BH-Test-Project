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
        public event Action<Player> OnPlayerSpawned;

        private readonly GameObject _playerPrefab;
        private readonly List<Transform> _spawnPoints;

        private int _currentPlayerId;

        public NetworkSpawnSystem(GameObject playerPrefab, List<Transform> spawnPoints)
        {
            _playerPrefab = playerPrefab;
            _spawnPoints = spawnPoints;
            NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreateCharacter);
        }
        
        public void SpawnNewPlayer()
        {
            CreatePlayerMessage createPlayerMessage = new CreatePlayerMessage()
            {
                SpawnPosition = _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)].position,
                Id = ++_currentPlayerId
            };

            NetworkClient.Send(createPlayerMessage);
        }

        private void OnCreateCharacter(NetworkConnectionToClient conn, CreatePlayerMessage message)
        {
            GameObject go = Object.Instantiate(_playerPrefab, message.SpawnPosition, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, go);
            OnPlayerSpawned?.Invoke(go.GetComponent<Player>());
        }
    }
}
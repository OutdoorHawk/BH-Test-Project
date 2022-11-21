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
        public event Action<PlayerBehavior, NetworkConnectionToClient> OnPlayerSpawned;

        private readonly GameObject _playerPrefab;
        private readonly List<Transform> _spawnPoints;

        public NetworkSpawnSystem(List<Transform> spawnPoints)
        {
            _spawnPoints = spawnPoints;
        }

        [ServerCallback]
        public void RegisterHandlers() =>
            NetworkServer.RegisterHandler<SpawnPlayerMessage>(OnCreateCharacter);

        public void SpawnNewPlayer()
        {
            SpawnPlayerMessage message = new SpawnPlayerMessage()
            {
                SpawnPosition = _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)].position,
            };

            NetworkClient.Send(message);
        }

        private void OnCreateCharacter(NetworkConnectionToClient conn, SpawnPlayerMessage message)
        {
            GameObject go = Object.Instantiate(_playerPrefab, message.SpawnPosition, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, go);
            OnPlayerSpawned?.Invoke(go.GetComponent<PlayerBehavior>(), conn);
        }
    }
}
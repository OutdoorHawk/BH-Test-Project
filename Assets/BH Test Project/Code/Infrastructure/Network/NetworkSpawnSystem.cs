using System.Collections.Generic;
using BH_Test_Project.Code.Runtime.Player;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkSpawnSystem
    {
        private readonly GameObject _playerPrefab;
        private readonly List<Transform> _spawnPoints;

        public NetworkSpawnSystem(List<Transform> spawnPoints)
        {
            _spawnPoints = spawnPoints;
        }

        public PlayerBehavior SpawnNewPlayer()
        {
            GameObject player = Object.Instantiate(_playerPrefab, GetAvailableSpawnPoint(), Quaternion.identity);
            GetAvailableSpawnPoint();
            return player.GetComponent<PlayerBehavior>();
        }

        private Vector3 GetAvailableSpawnPoint()
        {
            List<Transform> availableSpawnPoints = new List<Transform>();
            availableSpawnPoints.AddRange(_spawnPoints);
            for (int i = 0; i < _spawnPoints.Count; i++)
            {
                foreach (var conn in NetworkServer.connections.Values)
                {
                    if (conn.identity != null)
                    {
                        Vector3 playerPosition = conn.identity.transform.position;
                        if (playerPosition == _spawnPoints[i].position)
                            availableSpawnPoints.Remove(_spawnPoints[i]);
                    }
                }
            }

            return availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count - 1)].position;
        }
    }
}
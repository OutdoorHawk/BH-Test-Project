using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Network.Data;
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
        
        public void RegisterHandlers()
        {
            NetworkClient.RegisterHandler<GameRestartMessage>(OnGameRestarted);
        }

        public void UnregisterHandlers()
        {
            NetworkClient.UnregisterHandler<GameRestartMessage>();
        }

        private void OnGameRestarted(GameRestartMessage obj)
        {
            RespawnAllPlayers();
        }

        public void RespawnAllPlayers()
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity.TryGetComponent(out PlayerBehavior player))
                {
                    player.transform.position = GetAvailableSpawnPoint();
                    player.RpcPlayerRestart();
                }
            }
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
                            availableSpawnPoints.RemoveAt(i);
                    }
                }
            }

            return availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count - 1)].position;
        }
    }
}
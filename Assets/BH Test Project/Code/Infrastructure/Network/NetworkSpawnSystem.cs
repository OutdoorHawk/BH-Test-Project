using System.Collections.Generic;
using System.Linq;
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
                    player.transform.position = _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)].position;
                    player.RpcPlayerRestart();
                }
            }
        }

        private Vector3 GetAvailableSpawnPoint()
        {
            List<Vector3> availableSpawnPoints = _spawnPoints.Select(point => point.position).ToList();
            for (int i = 0; i < availableSpawnPoints.Count; i++)
            {
                foreach (var conn in NetworkServer.connections.Values)
                {
                    if (conn.identity != null)
                    {
                        Vector3 playerPosition = conn.identity.transform.position;
                        if (playerPosition == availableSpawnPoints[i])
                            availableSpawnPoints.RemoveAt(i);
                    }
                }
            }

            foreach (var point in availableSpawnPoints) 
                Debug.Log(point);

            return availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count - 1)];
        }
    }
}
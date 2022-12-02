using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Runtime.Lobby;
using BH_Test_Project.Code.Runtime.Player.UI;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services
{
    public class SceneContextService : ISceneContextService
    {
        private SceneContext _sceneContext;
        private List<Transform> _spawnPoints;
        private NetworkPlayerSystem _playerSystem;
        private LobbyMenuWindow _lobbyMenuWindow;

        public void CollectSceneContext()
        {
            _sceneContext = Object.FindObjectOfType<SceneContext>(true);
            _playerSystem = _sceneContext.PlayerSystem;
            CollectSceneSpawnPoints();
        }

        private void CollectSceneSpawnPoints()
        {
            _spawnPoints = new List<Transform>();
            for (int i = 0; i < _sceneContext.SpawnPointsParent.childCount; i++)
            {
                Transform spawn = _sceneContext.SpawnPointsParent.GetChild(i);
                _spawnPoints.Add(spawn);
            }
        }

        public void SetLobbyMenu(LobbyMenuWindow lobby) => 
            _lobbyMenuWindow = lobby;

        public List<Transform> GetSceneSpawnPoints()
            => _spawnPoints;

        public NetworkPlayerSystem GetPlayerSystem()
            => _playerSystem;

        public LobbyMenuWindow GetLobbyMenuWindow() => 
            _lobbyMenuWindow;
    }
}
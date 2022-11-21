using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.MainMenu.Network
{
    public class GameNetworkManager : NetworkRoomManager
    {
        private IGameStateMachine _gameStateMachine;
        private ISceneContextService _sceneContextService;

        private NetworkSpawnSystem _spawnSystem;
        private NetworkPlayerSystem _playerSystem;

        private List<Transform> _spawnPoints;

        public void Init(IGameStateMachine gameStateMachine, ISceneContextService sceneContextService)
        {
            _sceneContextService = sceneContextService;
            _gameStateMachine = gameStateMachine;
        }

        public void CreateLobbyAsHost()
        {
            if (!NetworkServer.active)
                StartHost();
        }

        public void CreateLobbyAsClient(string address)
        {
            networkAddress = address;
            if (!NetworkClient.active && !NetworkServer.active)
                StartClient();
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn,
            GameObject roomPlayer,
            GameObject gamePlayer)
        {
            _gameStateMachine.Enter<GameLoopState>();
            InitGameLevel();
            return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
        }

        public void InitGameLevel()
        {
            _spawnPoints = _sceneContextService.GetSceneSpawnPoints();
            CreateSystems();
            Subscribe();
        }

        private void CreateSystems()
        {
            _spawnSystem = new NetworkSpawnSystem(playerPrefab, _spawnPoints);
            _playerSystem = new NetworkPlayerSystem();
            _spawnSystem.RegisterHandlers();
            _playerSystem.RegisterHandlers();
        }

        private void Subscribe()
        {
            _spawnSystem.OnPlayerSpawned += _playerSystem.AddNewPlayer;
        }

        private void CleanUp()
        {
            _spawnSystem.OnPlayerSpawned -= _playerSystem.AddNewPlayer;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            CleanUp();
        }
    }
}
using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;
using BH_Test_Project.Code.Runtime.Player;
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

        public void Init(IGameStateMachine gameStateMachine, ISceneContextService sceneContextService)
        {
            _sceneContextService = sceneContextService;
            _gameStateMachine = gameStateMachine;
            CreateSystems();
        }

        private void CreateSystems()
        {
            _playerSystem = new NetworkPlayerSystem();
            _playerSystem.RegisterHandlers();
        }

        public void CreateLobbyAsHost()
        {
            if (!NetworkServer.active)
                StartHost();
        }

        public void JoinLobbyAsClient(string address)
        {
            networkAddress = address;
            if (!NetworkClient.active && !NetworkServer.active)
                StartClient();
        }

        public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn,
            GameObject roomPlayer)
        {
            PlayerBehavior playerBehavior = Instantiate(playerPrefab).GetComponent<PlayerBehavior>();
            _playerSystem.AddNewPlayer(playerBehavior, conn);
            return playerBehavior.gameObject;
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn,
            GameObject roomPlayer, GameObject gamePlayer)
        {
            _gameStateMachine.Enter<GameLoopState>(); // todo Rework with actions
            InitGameLevel();
            return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
        }

        private void InitGameLevel()
        {
            List<Transform> spawnPoints = _sceneContextService.GetSceneSpawnPoints();
            _spawnSystem = new NetworkSpawnSystem(spawnPoints);
        }
    }
}
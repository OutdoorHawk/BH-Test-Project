using System.Collections;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.sceneLoaded += HandleGameLevelLoaded;
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

        public override void OnStartClient()
        {
            base.OnStartClient();
            NetworkClient.RegisterHandler<GameRestartMessage>(OnGameRestarted);
        }

        private void HandleGameLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (SceneManager.GetActiveScene().name != Constants.GAME_LEVEL_NAME)
                return;

            _gameStateMachine.Enter<GameLoopState>();
            InitGameLevel();
        }

        private void InitGameLevel()
        {
            _playerSystem = _sceneContextService.GetPlayerSystem();
        }

        private void OnGameRestarted(GameRestartMessage obj)
        {
            ServerChangeScene(GameplayScene);
            _gameStateMachine.Enter<GameLoopState>();
        }
        
        public override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.sceneLoaded -= HandleGameLevelLoaded;
        }
    }
}
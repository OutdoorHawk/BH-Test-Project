using System.Collections;
using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;
using BH_Test_Project.Code.Runtime.Player;
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

        private void HandleGameLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (SceneManager.GetActiveScene().name != Constants.GAME_LEVEL_NAME)
                return;
            
            _gameStateMachine.Enter<GameLoopState>(); // todo Rework with actions
            
            InitGameLevel();
        }

        private void InitGameLevel()
        {
            _playerSystem = _sceneContextService.GetPlayerSystem();
            startPositions = _sceneContextService.GetSceneSpawnPoints();
            _playerSystem.OnGameEnd += RestartGame;
        }

        private void RestartGame()
        {
            StartCoroutine(RestartGameRoutine());
        }

        private IEnumerator RestartGameRoutine()
        {
            _playerSystem.OnGameEnd -= RestartGame;
            yield return new WaitForSeconds(4.5f);
            ServerChangeScene(GameplayScene);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.sceneLoaded -= HandleGameLevelLoaded;
        }
    }
}
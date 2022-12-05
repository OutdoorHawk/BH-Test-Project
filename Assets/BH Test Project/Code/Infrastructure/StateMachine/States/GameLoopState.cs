using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.PlayerFactory;
using BH_Test_Project.Code.Infrastructure.Services.SceneLoaderService;
using BH_Test_Project.Code.Infrastructure.Services.UI;
using BH_Test_Project.Code.Runtime.Player.UI;
using BH_Test_Project.Code.StaticData;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneContextService _sceneContextService;
        private readonly IUIFactory _uiFactory;
        private readonly ISceneLoader _sceneLoader;
        private readonly INetworkManagerService _networkManagerService;
        private readonly IPlayerFactory _playerFactory;
        private NetworkPlayerSystem _playerSystem;

        public GameLoopState(IStaticDataService staticDataService,
            ISceneContextService sceneContextService, IUIFactory uiFactory, ISceneLoader sceneLoader,
            INetworkManagerService networkManagerService, IPlayerFactory playerFactory)
        {
            _staticDataService = staticDataService;
            _sceneContextService = sceneContextService;
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
            _networkManagerService = networkManagerService;
            _playerFactory = playerFactory;
        }

        public void Enter()
        {
            SceneManager.sceneLoaded += OnLoaded;
        }

        private void OnLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            InitGameLevel();
            Subscribe();
        }

        private void Subscribe()
        {
     
        }

        private void Unsubscribe()
        {
            
        }
        
        private void InitGameLevel()
        {
            _sceneContextService.CollectSceneContext();
            NetworkManager.startPositions = _sceneContextService.GetSceneSpawnPoints();
            InitPlayerSystem();
        }

        private void InitPlayerSystem()
        {
            _playerSystem = _sceneContextService.GetPlayerSystem();
            _playerSystem.RegisterHandlers();
            WorldStaticData worldStaticData = _staticDataService.GetWorldStaticData();
            PlayerStaticData playerStaticData = _staticDataService.GetPlayerStaticData();
            PlayerHUD playerHUD = _uiFactory.CreatePlayerHUD();
            _playerSystem.Init(playerHUD, worldStaticData, playerStaticData);
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= OnLoaded;
            _uiFactory.ClearUIRoot();
            _playerSystem.UnregisterHandlers();
        }
    }
}
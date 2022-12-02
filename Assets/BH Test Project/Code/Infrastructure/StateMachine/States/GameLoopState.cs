using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.Services.SceneLoaderService;
using BH_Test_Project.Code.Runtime.Player.UI;
using BH_Test_Project.Code.StaticData;
using Mirror;
using UnityEngine.SceneManagement;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneContextService _sceneContextService;
        private readonly IUIFactory _uiFactory;
        private NetworkPlayerSystem _playerSystem;

        public GameLoopState(IStaticDataService staticDataService,
            ISceneContextService sceneContextService, IUIFactory uiFactory, ISceneLoader sceneLoader)
        {
            _staticDataService = staticDataService;
            _sceneContextService = sceneContextService;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
           InitGameLevel();
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
            _uiFactory.ClearUIRoot();
            _playerSystem.UnregisterHandlers();
        }
    }
}
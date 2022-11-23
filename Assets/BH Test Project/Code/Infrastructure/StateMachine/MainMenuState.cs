using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Runtime.MainMenu.Network;
using BH_Test_Project.Code.Runtime.MainMenu.Windows;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class MainMenuState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneContextService _sceneContextService;
        private GameNetworkManager _gameNetworkManager;

        public MainMenuState(IGameStateMachine gameStateMachine, IUIFactory uiFactory,
            IStaticDataService staticDataService, ISceneContextService sceneContextService)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _staticDataService = staticDataService;
            _sceneContextService = sceneContextService;
        }

        public void Enter()
        {
            _uiFactory.CreateUiRoot();
            InitNetworkManager();
            InitMainMenu();
        }

        private void InitMainMenu()
        {
            MainMenuWindow mainMenuWindow = _uiFactory.CreateMainMenuWindow();
            mainMenuWindow.Init(_gameNetworkManager);
        }

        private void InitNetworkManager()
        {
            if (_gameNetworkManager == null)
                _gameNetworkManager = Object.Instantiate(_staticDataService.GetLobbyNetworkManager());
            _gameNetworkManager.Init(_gameStateMachine, _sceneContextService, _uiFactory);
        }

        public void Exit()
        {
           _uiFactory.ClearUIRoot();
        }
    }
}
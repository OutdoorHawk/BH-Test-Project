using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Runtime.MainMenu.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class MainMenuState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IStaticDataService _staticDataService;
        private GameNetworkManager _gameNetworkManager;

        public MainMenuState(IGameStateMachine gameStateMachine, IUIFactory uiFactory,
            IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_NAME);
            InitNetworkManager();
            InitMainMenu();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
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
            _gameNetworkManager.Init(_gameStateMachine);
        }

        public void Exit()
        {
            _uiFactory.ClearUIRoot();
        }
    }
}
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
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_NAME);
            SceneManager.sceneLoaded += OnLoaded;
        }

        private void OnLoaded(Scene arg0, LoadSceneMode arg1)
        {
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
            _gameNetworkManager.Init(_gameStateMachine);
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= OnLoaded;
            _uiFactory.ClearUIRoot();
        }
    }
}
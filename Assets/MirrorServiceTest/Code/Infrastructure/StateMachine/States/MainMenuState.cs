using Mirror;
using MirrorServiceTest.Code.Infrastructure.Data;
using MirrorServiceTest.Code.Infrastructure.Services.Network;
using MirrorServiceTest.Code.Infrastructure.Services.SceneLoaderService;
using MirrorServiceTest.Code.Infrastructure.Services.UI;
using MirrorServiceTest.Code.Runtime.MainMenu.Windows;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.StateMachine.States
{
    public class MainMenuState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IGameNetworkService _gameNetworkService;
        private readonly ISceneLoader _sceneLoader;

        public MainMenuState(IGameStateMachine gameStateMachine, IUIFactory uiFactory,
            IGameNetworkService gameNetworkService,
            ISceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _gameNetworkService = gameNetworkService;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            _sceneLoader.LoadScene(Constants.MAIN_MENU_SCENE_NAME, OnLoaded);
        }

        private void OnLoaded()
        {
            InitMainMenu();
            if (NetworkServer.active) 
                NetworkServer.Shutdown();
        }

        private void InitMainMenu()
        {
            MainMenuWindow mainMenuWindow = _uiFactory.CreateMainMenuWindow();
            mainMenuWindow.Init(_gameNetworkService,_gameStateMachine);
        }

        public void Exit()
        {
            _uiFactory.ClearUIRoot();
        }
    }
}
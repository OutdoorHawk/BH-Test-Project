using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.SceneLoaderService;
using BH_Test_Project.Code.Runtime.MainMenu.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class MainMenuState : IState
    {
        private readonly IUIFactory _uiFactory;
        private readonly INetworkManagerService _networkManagerService;
        private readonly ISceneLoader _sceneLoader;

        public MainMenuState(IUIFactory uiFactory, INetworkManagerService networkManagerService,
            ISceneLoader sceneLoader)
        {
            _uiFactory = uiFactory;
            _networkManagerService = networkManagerService;
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
        }

        private void InitMainMenu()
        {
            MainMenuWindow mainMenuWindow = _uiFactory.CreateMainMenuWindow();
            mainMenuWindow.Init(_networkManagerService);
        }

        public void Exit()
        {
            _uiFactory.ClearUIRoot();
        }
    }
}
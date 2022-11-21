using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.StaticData;
using BH_Test_Project.Code.Runtime.MainMenu.Windows;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services
{
    public class UIFactory : IUIFactory
    {
        private Transform _uiRoot;
        private readonly IStaticDataService _staticDataService;

        public UIFactory(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public MainMenuWindow CreateMainMenuWindow()
        {
            WindowConfig windowPrefab = _staticDataService.GetWindow(WindowID.MainMenu);
            MainMenuWindow window = Object.Instantiate(windowPrefab.WindowPrefab, _uiRoot)
                .GetComponent<MainMenuWindow>();
            return window;
        }

        public void CreateUiRoot()
        {
            WindowConfig root = _staticDataService.GetWindow(WindowID.UiRoot);
            _uiRoot = Object.Instantiate(root.WindowPrefab).transform;
        }
    }
}
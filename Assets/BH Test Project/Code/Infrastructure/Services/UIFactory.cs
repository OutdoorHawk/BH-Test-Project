using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.StaticData;
using BH_Test_Project.Code.UI;
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

        public void CreateWindow(WindowID id)
        {
            WindowConfig windowPrefab = _staticDataService.GetWindow(id);
            WindowBase window = Object.Instantiate(windowPrefab.Prefab, _uiRoot);
        }

        public void CreateUiRoot()
        {
            WindowConfig root = _staticDataService.GetWindow(WindowID.UiRoot);
            _uiRoot = Object.Instantiate(root.Prefab).transform;
        }
    }
}
using Mirror;
using MirrorServiceTest.Code.Infrastructure.Data;
using MirrorServiceTest.Code.Infrastructure.Services.StaticData;
using MirrorServiceTest.Code.Infrastructure.StaticData;
using MirrorServiceTest.Code.Runtime.Lobby;
using MirrorServiceTest.Code.Runtime.MainMenu.Windows;
using MirrorServiceTest.Code.Runtime.Player.UI;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.Services.UI
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

        public LobbyMenuWindow CreateLobbyMenuWindow()
        {
            WindowConfig windowPrefab = _staticDataService.GetWindow(WindowID.Lobby);
            LobbyMenuWindow window = Object.Instantiate(windowPrefab.WindowPrefab, _uiRoot)
                .GetComponent<LobbyMenuWindow>();
            return window;
        }

        public PlayerHUD CreatePlayerHUD(NetworkConnectionToClient conn)
        {
            WindowConfig windowPrefab = _staticDataService.GetWindow(WindowID.PlayerHUD);
            PlayerHUD window = Object.Instantiate(windowPrefab.WindowPrefab, _uiRoot)
                .GetComponent<PlayerHUD>();
            return window;
        }

        public void CreateUiRoot()
        {
            if (_uiRoot != null)
                Object.Destroy(_uiRoot.gameObject);
            WindowConfig config = _staticDataService.GetWindow(WindowID.UiRoot);
            GameObject uiRoot = Object.Instantiate(config.WindowPrefab.gameObject);
            Object.DontDestroyOnLoad(uiRoot);
            _uiRoot = uiRoot.transform;
        }

        public void ClearUIRoot()
        {
            for (int i = 0; i < _uiRoot.childCount; i++)
            {
                if (_uiRoot.GetChild(i).gameObject.TryGetComponent(out RoomPlayer roomPlayer))
                    roomPlayer.transform.SetParent(null);
                else
                    Object.Destroy(_uiRoot.GetChild(i).gameObject);
            }
        }
    }
}
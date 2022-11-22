using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.StaticData;
using BH_Test_Project.Code.Runtime.MainMenu.Network;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services
{
    public class StaticDataService : IStaticDataService
    {
        private const string GAME_STATIC_DATA_PATH = "GameStaticData";

        private readonly Dictionary<WindowID, WindowConfig> _windows = new();
        private GameNetworkManager _gameNetworkManager;
        private NetworkManager _networkManager;
        public GameStaticData Data { get; private set; }

        public void Load()
        {
            Data = Resources.Load<GameStaticData>(GAME_STATIC_DATA_PATH);
            LoadWindows();
            LoadNetworkManager();
        }

        public WindowConfig GetWindow(WindowID id)
        {
            return _windows.TryGetValue(id, out var windowConfig) ? windowConfig : null;
        }

        public GameNetworkManager GetLobbyNetworkManager()
        {
            return _gameNetworkManager;
        }

        private void LoadWindows()
        {
            foreach (var window in Data.Windows)
                _windows.Add(window.ID, window);
        }

        private void LoadNetworkManager()
        {
            _gameNetworkManager = Data.ManagerPrefab;
        }
    }
}
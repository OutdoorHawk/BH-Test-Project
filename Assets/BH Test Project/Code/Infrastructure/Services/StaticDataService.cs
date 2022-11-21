using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.StaticData;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services
{
    public class StaticDataService : IStaticDataService
    {
        private const string GAME_STATIC_DATA_PATH = "GameStaticData";

        private readonly Dictionary<WindowID, WindowConfig> _windows = new();
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

        public NetworkManager GetNetworkManager()
        {
            return _networkManager;
        }

        private void LoadWindows()
        {
            foreach (var window in Data.Windows)
                _windows.Add(window.ID, window);
        }

        private void LoadNetworkManager()
        {
            _networkManager = Data.ManagerPrefab;
        }
    }
}
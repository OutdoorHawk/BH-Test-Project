using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.StaticData;
using BH_Test_Project.Code.Runtime.MainMenu.Network;
using BH_Test_Project.Code.StaticData;
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
        private PlayerStaticData _playerStaticData;
        private WorldStaticData _worldStaticData;
        private GameStaticData _data;

        public void Load()
        {
            _data = Resources.Load<GameStaticData>(GAME_STATIC_DATA_PATH);
            LoadWindows();
            LoadNetworkManager();
            LoadPlayerStaticData();
            LoadWorldStaticData();
        }

        private void LoadWindows()
        {
            foreach (var window in _data.Windows)
                _windows.Add(window.ID, window);
        }

        private void LoadNetworkManager() => 
            _gameNetworkManager = _data.ManagerPrefab;

        private void LoadPlayerStaticData() => 
            _playerStaticData = _data.PlayerStaticData;

        private void LoadWorldStaticData() => 
            _worldStaticData = _data.WorldStaticData;

        public WindowConfig GetWindow(WindowID id) => 
            _windows.TryGetValue(id, out var windowConfig) ? windowConfig : null;

        public GameNetworkManager GetLobbyNetworkManager() => 
            _gameNetworkManager;

        public PlayerStaticData GetPlayerStaticData() => 
            _playerStaticData;

        public WorldStaticData GetWorldStaticData() => 
            _worldStaticData;
    }
}
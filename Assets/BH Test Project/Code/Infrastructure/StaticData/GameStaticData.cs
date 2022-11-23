using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.StaticData;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "GameStaticData", menuName = "Static Data/GameStaticData")]
    public class GameStaticData : ScriptableObject
    {
        [SerializeField] private WindowConfig[] _windows;
        [SerializeField] private GameNetworkManager _gameManagerPrefab;
        [SerializeField] private NetworkPlayerSystem _networkPlayerSystem;
        [SerializeField, Header("PlayerStaticData")] private PlayerStaticData _playerStaticData;
        [SerializeField, Header("WorldStaticData")] private WorldStaticData _worldStaticData;

        public IEnumerable<WindowConfig> Windows => _windows;
        public GameNetworkManager ManagerPrefab => _gameManagerPrefab;
        public PlayerStaticData PlayerStaticData => _playerStaticData;
        public WorldStaticData WorldStaticData => _worldStaticData;
        public NetworkPlayerSystem PlayerSystem => _networkPlayerSystem;
    }
}
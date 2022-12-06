using System.Collections.Generic;
using MirrorServiceTest.Code.Infrastructure.Services.Network;
using MirrorServiceTest.Code.StaticData;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "GameStaticData", menuName = "Static Data/GameStaticData")]
    public class GameStaticData : ScriptableObject
    {
        [SerializeField] private WindowConfig[] _windows;
        [SerializeField] private GameNetworkService servicePrefab;
        [SerializeField, Header("PlayerStaticData")] private PlayerStaticData _playerStaticData;
        [SerializeField, Header("WorldStaticData")] private WorldStaticData _worldStaticData;

        public IEnumerable<WindowConfig> Windows => _windows;
        public GameNetworkService ServicePrefab => servicePrefab;
        public PlayerStaticData PlayerStaticData => _playerStaticData;
        public WorldStaticData WorldStaticData => _worldStaticData;
    }
}
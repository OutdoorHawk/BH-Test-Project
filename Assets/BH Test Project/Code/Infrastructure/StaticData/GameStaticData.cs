using BH_Test_Project.Code.Runtime.MainMenu.Network;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "GameStaticData", menuName = "Static Data/GameStaticData")]
    public class GameStaticData : ScriptableObject
    {
        [SerializeField] private WindowConfig[] _windows;
        [SerializeField] private LobbyNetworkManager _lobbyManagerPrefab;

        public WindowConfig[] Windows => _windows;

        public LobbyNetworkManager ManagerPrefab => _lobbyManagerPrefab;
    }
}

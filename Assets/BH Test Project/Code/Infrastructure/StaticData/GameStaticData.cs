using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "GameStaticData", menuName = "Static Data/GameStaticData")]
    public class GameStaticData : ScriptableObject
    {
        [SerializeField] private WindowConfig[] _windows;
        [SerializeField] private NetworkManager _networkManagerPrefab;

        public WindowConfig[] Windows => _windows;

        public NetworkManager ManagerPrefab => _networkManagerPrefab;
    }
}

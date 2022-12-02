using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Runtime.Player.UI;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services
{
    public class SceneContext : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPointsParent;
        [SerializeField] private NetworkPlayerSystem _playerSystem;

        public Transform SpawnPointsParent => _spawnPointsParent;

        public NetworkPlayerSystem PlayerSystem => _playerSystem;
        
    }
}

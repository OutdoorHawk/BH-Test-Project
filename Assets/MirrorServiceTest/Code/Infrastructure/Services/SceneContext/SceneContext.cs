using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.Services.SceneContext
{
    public class SceneContext : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPointsParent;

        public Transform SpawnPointsParent => _spawnPointsParent;
        
        
    }
}

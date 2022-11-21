using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services
{
    public class SceneContextService : ISceneContextService
    {
        private SceneContext _sceneContext;
        private List<Transform> _spawnPoints;

        public void InitSceneContext()
        {
            _sceneContext = Object.FindObjectOfType<SceneContext>();
            CollectSceneSpawnPoints();
        }

        private void CollectSceneSpawnPoints() => _spawnPoints = _sceneContext.SpawnPointsParent.GetComponentsInChildren<Transform>().ToList();

        public List<Transform> GetSceneSpawnPoints() => _spawnPoints;
    }
}
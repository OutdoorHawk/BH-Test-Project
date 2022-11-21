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
            _sceneContext = Object.FindObjectOfType<SceneContext>(true);
            CollectSceneSpawnPoints();
        }

        private void CollectSceneSpawnPoints()
        {
            _spawnPoints = new List<Transform>();
            for (int i = 0; i < _sceneContext.SpawnPointsParent.childCount; i++)
            {
                Transform spawn = _sceneContext.SpawnPointsParent.GetChild(i);
                _spawnPoints.Add(spawn);
            }
        }

        public List<Transform> GetSceneSpawnPoints() 
            => _spawnPoints;
    }
}
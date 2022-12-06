using System.Collections.Generic;
using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Runtime.Lobby;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.Services.SceneContext
{
    public interface ISceneContextService : IService
    {
        void CollectSceneContext();
        List<Transform> GetSceneSpawnPoints();
    }
}
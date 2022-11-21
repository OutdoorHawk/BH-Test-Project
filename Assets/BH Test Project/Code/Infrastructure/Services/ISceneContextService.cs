using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.DI;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services
{
    public interface ISceneContextService: IService
    {
        void InitSceneContext();
        List<Transform> GetSceneSpawnPoints();
    }
}
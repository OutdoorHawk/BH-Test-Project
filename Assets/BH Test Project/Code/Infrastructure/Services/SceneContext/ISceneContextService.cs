using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Runtime.Lobby;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services.SceneContext
{
    public interface ISceneContextService : IService
    {
        void CollectSceneContext();
        List<Transform> GetSceneSpawnPoints();
        void SetLobbyMenu(LobbyMenuWindow lobby);
        LobbyMenuWindow GetLobbyMenuWindow();
    }
}
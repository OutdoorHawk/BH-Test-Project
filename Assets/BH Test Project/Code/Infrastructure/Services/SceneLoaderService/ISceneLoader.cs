using System;
using BH_Test_Project.Code.Infrastructure.DI;

namespace BH_Test_Project.Code.Infrastructure.Services.SceneLoaderService
{
    public interface ISceneLoader : IService
    {
        void LoadScene(string sceneName, Action onLoaded = null);
    }
}
using System;
using MirrorServiceTest.Code.Infrastructure.DI;

namespace MirrorServiceTest.Code.Infrastructure.Services.SceneLoaderService
{
    public interface ISceneLoader : IService
    {
        void LoadScene(string sceneName, Action onLoaded = null);
    }
}
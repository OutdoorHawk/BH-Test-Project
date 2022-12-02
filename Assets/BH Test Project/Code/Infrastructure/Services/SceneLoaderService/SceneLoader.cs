using System;
using System.Collections;
using BH_Test_Project.Code.Infrastructure.Services.CoroutineRunner;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BH_Test_Project.Code.Infrastructure.Services.SceneLoaderService
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void LoadScene(string sceneName, Action OnLoaded = null)
        {
            _coroutineRunner.StartCoroutine(LoadingScreenStartRoutine(sceneName, OnLoaded));
        }

        private IEnumerator LoadingScreenStartRoutine(string sceneName, Action onLoaded)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
                yield return null;

            onLoaded?.Invoke();
        }
    }

}
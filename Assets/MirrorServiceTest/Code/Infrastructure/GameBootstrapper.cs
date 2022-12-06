using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Infrastructure.Services.CoroutineRunner;
using MirrorServiceTest.Code.Infrastructure.StateMachine;
using MirrorServiceTest.Code.Infrastructure.StateMachine.States;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private IGameStateMachine _gameStateMachine;

        private void Awake()
        {
            _gameStateMachine = new GameStateMachine(DIContainer.Container, this);
            _gameStateMachine.Enter<BootstrapState>();
            DontDestroyOnLoad(gameObject);
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}
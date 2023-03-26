using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Infrastructure.Services.CoroutineRunner;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService;
using MirrorServiceTest.Code.Infrastructure.Services.UpdateBehavior;
using MirrorServiceTest.Code.Infrastructure.StateMachine;
using MirrorServiceTest.Code.Infrastructure.StateMachine.States;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private RecordingService _recordingService;
        [SerializeField] private UpdateBehaviourService _updateBehaviourService;

        private IGameStateMachine _gameStateMachine;

        private void Awake()
        {
            _gameStateMachine = new GameStateMachine(DIContainer.Container, this, _recordingService,_updateBehaviourService);
            _gameStateMachine.Enter<BootstrapState>();
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(_recordingService.gameObject);
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}
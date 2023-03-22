using Mirror;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService;
using MirrorServiceTest.Code.Infrastructure.Services.SceneContext;
using MirrorServiceTest.Code.Infrastructure.Services.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MirrorServiceTest.Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly ISceneContextService _sceneContextService;
        private readonly IRecordingService _recordingService;
        private readonly IUIFactory _uiFactory;

        public GameLoopState(ISceneContextService sceneContextService, IUIFactory uiFactory,
            IRecordingService recordingService)
        {
            _recordingService = recordingService;
            _sceneContextService = sceneContextService;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            Cursor.lockState = CursorLockMode.Locked;
            SceneManager.sceneLoaded += OnLoaded;
            _recordingService.Initialize();
        }

        private void OnLoaded(Scene arg0, LoadSceneMode loadSceneMode)
        {
            InitGameLevel();
        }

        private void InitGameLevel()
        {
            _uiFactory.ClearUIRoot();
            _sceneContextService.CollectSceneContext();
            NetworkManager.startPositions = _sceneContextService.GetSceneSpawnPoints();
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= OnLoaded;
            _uiFactory.ClearUIRoot();
            _recordingService.CleanUp();
        }
    }
}
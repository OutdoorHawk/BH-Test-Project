using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.Services.SceneContext;
using BH_Test_Project.Code.Infrastructure.Services.UI;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly ISceneContextService _sceneContextService;
        private readonly IUIFactory _uiFactory;

        public GameLoopState(ISceneContextService sceneContextService, IUIFactory uiFactory)
        {
            _sceneContextService = sceneContextService;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            Cursor.lockState = CursorLockMode.Locked;
            SceneManager.sceneLoaded += OnLoaded;
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
        }
    }
}
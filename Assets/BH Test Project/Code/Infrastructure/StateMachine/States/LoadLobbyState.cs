using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Services.SceneLoaderService;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class LoadLobbyState : IState
    {
        private readonly ISceneLoader _sceneLoader;

        public LoadLobbyState(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.LoadScene(Constants.LOBBY_SCENE_NAME, OnLoaded);
        }

        private void OnLoaded()
        {
            
        }

        public void Exit()
        {
        }
    }
}
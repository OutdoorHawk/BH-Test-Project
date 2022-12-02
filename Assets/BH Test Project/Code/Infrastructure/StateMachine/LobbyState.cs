using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.Services.SceneLoaderService;
using BH_Test_Project.Code.Runtime.Lobby;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class LobbyState : IState
    {
        private readonly IUIFactory _uiFactory;
        private readonly ISceneLoader _sceneLoader;

        private LobbyMenuWindow _lobbyMenuWindow;

        public LobbyState(IUIFactory uiFactory, ISceneLoader sceneLoader)
        {
            _uiFactory = uiFactory;
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
            _uiFactory.ClearUIRoot();
        }
    }
}
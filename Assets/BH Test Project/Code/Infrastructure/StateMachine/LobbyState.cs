using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.SceneLoaderService;
using BH_Test_Project.Code.Runtime.Lobby;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class LobbyState : IState
    {
        private readonly IUIFactory _uiFactory;
        private readonly ISceneLoader _sceneLoader;
        private readonly INetworkManagerService _networkManagerService;

        private LobbyMenuWindow _lobbyMenuWindow;

        public LobbyState(IUIFactory uiFactory, ISceneLoader sceneLoader,
            INetworkManagerService networkManagerService)
        {
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
            _networkManagerService = networkManagerService;
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
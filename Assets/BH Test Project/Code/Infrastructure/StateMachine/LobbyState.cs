using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Runtime.Lobby;
using UnityEngine.SceneManagement;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class LobbyState : IState
    {
        private readonly IUIFactory _uiFactory;

        private LobbyMenuWindow _lobbyMenuWindow;

        public LobbyState(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            SceneManager.LoadScene(Constants.LOBBY_SCENE_NAME);
        }

        public void Exit()
        {
            _uiFactory.ClearUIRoot();
        }
    }
}
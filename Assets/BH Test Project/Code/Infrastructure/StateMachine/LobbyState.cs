using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class LobbyState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly IUIFactory _uiFactory;

        private LobbyMenuWindow _lobbyMenuWindow;

        public LobbyState(IGameStateMachine gameStateMachine, IStaticDataService staticDataService,
            IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            _lobbyMenuWindow = _uiFactory.CreateLobbyMenuWindow();
            _lobbyMenuWindow.InitLobby(NetworkClient.isHostClient);
        }

        public void Exit()
        {
            
        }
    }
}
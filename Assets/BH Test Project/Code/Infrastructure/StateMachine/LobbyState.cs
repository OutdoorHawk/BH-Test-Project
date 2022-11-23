using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class LobbyState : IState, IPayloadedState<int>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly ISceneContextService _sceneContextService;

        private LobbyMenuWindow _lobbyMenuWindow;

        public LobbyState(IGameStateMachine gameStateMachine, IUIFactory uiFactory,
            ISceneContextService sceneContextService)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _sceneContextService = sceneContextService;
        }

        public void Enter()
        {
            
        }

        public void Enter(int minPlayersToStartGame)
        {
            
            _lobbyMenuWindow = _uiFactory.CreateLobbyMenuWindow();
            if (NetworkServer.active)
                NetworkServer.Spawn(_lobbyMenuWindow.gameObject);
            _lobbyMenuWindow.InitLobby(NetworkClient.isHostClient, minPlayersToStartGame);
            _sceneContextService.SetLobbyMenu(_lobbyMenuWindow);
        }

        public void Exit()
        {
            _uiFactory.ClearUIRoot();
        }
    }
}
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.PlayerFactory;
using BH_Test_Project.Code.Infrastructure.Services.UI;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class LobbyState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IGameNetworkService _gameNetworkService;
        private readonly IPlayerFactory _playerFactory;

        private LobbyMenuWindow _lobbyMenuWindow;

        public LobbyState(GameStateMachine gameStateMachine, IUIFactory uiFactory,
            IGameNetworkService gameNetworkService,
            IPlayerFactory playerFactory)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _gameNetworkService = gameNetworkService;
            _playerFactory = playerFactory;
        }

        public void Enter()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _gameNetworkService.OnServerReadyEvent += CreateNewRoomPlayer;
            _gameNetworkService.OnRoomClientEnterEvent += UpdatePlayersUI;
            _gameNetworkService.OnRoomClientSceneChangedEvent += CheckSceneLoaded;
        }

        private void Unsubscribe()
        {
            _gameNetworkService.OnServerReadyEvent -= CreateNewRoomPlayer;
            _gameNetworkService.OnRoomClientEnterEvent -= UpdatePlayersUI;
            _gameNetworkService.OnRoomClientSceneChangedEvent -= CheckSceneLoaded;
        }

        [Server]
        private void CreateNewRoomPlayer(NetworkConnectionToClient conn)
        {
            RoomPlayer player = _gameNetworkService.RoomPlayerPrefab;
            _playerFactory.CreateRoomPlayer(conn, player);
        }

        [Client]
        private void UpdatePlayersUI()
        {
            foreach (var player in _gameNetworkService.PlayersInRoom)
            {
                if (player is RoomPlayer roomPlayer)
                    roomPlayer.UpdatePlayerUI();
            }
        }

        private void CheckSceneLoaded(string sceneName)
        {
            if (sceneName == Constants.GAME_SCENE_NAME)
                _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            Unsubscribe();
            _uiFactory.ClearUIRoot();
        }
    }
}
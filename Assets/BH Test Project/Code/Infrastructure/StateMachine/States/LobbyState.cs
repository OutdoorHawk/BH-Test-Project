using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.PlayerFactory;
using BH_Test_Project.Code.Infrastructure.Services.UI;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class LobbyState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly INetworkManagerService _networkManagerService;
        private readonly IPlayerFactory _playerFactory;

        private LobbyMenuWindow _lobbyMenuWindow;

        public LobbyState(GameStateMachine gameStateMachine, IUIFactory uiFactory,
            INetworkManagerService networkManagerService,
            IPlayerFactory playerFactory)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _networkManagerService = networkManagerService;
            _playerFactory = playerFactory;
        }
        
        public void Enter()
        {
            _networkManagerService.OnServerReadyEvent += CreateNewRoomPlayer;
            _networkManagerService.OnRoomClientEnterEvent += UpdatePlayersUI;
            _networkManagerService.OnRoomClientSceneChangedEvent += CheckSceneLoaded;
        }

        [Server]
        private void CreateNewRoomPlayer(NetworkConnectionToClient conn)
        {
            RoomPlayer player = _networkManagerService.RoomPlayerPrefab;
            _playerFactory.CreateRoomPlayer(conn, player);
        }

        [Client]
        private void UpdatePlayersUI()
        {
            foreach (var player in _networkManagerService.PlayersInRoom)
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
            _networkManagerService.OnServerReadyEvent -= CreateNewRoomPlayer;
            _networkManagerService.OnRoomClientEnterEvent -= UpdatePlayersUI;
            _networkManagerService.OnRoomClientSceneChangedEvent -= CheckSceneLoaded;
            _uiFactory.ClearUIRoot();
        }
    }
}
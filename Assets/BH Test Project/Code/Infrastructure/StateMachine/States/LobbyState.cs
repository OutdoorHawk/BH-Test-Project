using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.PlayerFactory;
using BH_Test_Project.Code.Infrastructure.Services.UI;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class LobbyState : IState, IPayloadedState<NetworkDataPayload>
    {
        private readonly IUIFactory _uiFactory;
        private readonly INetworkManagerService _networkManagerService;
        private readonly IPlayerFactory _playerFactory;

        private LobbyMenuWindow _lobbyMenuWindow;
        private NetworkDataPayload _payload;

        public LobbyState(IUIFactory uiFactory, INetworkManagerService networkManagerService,
            IPlayerFactory playerFactory)
        {
            _uiFactory = uiFactory;
            _networkManagerService = networkManagerService;
            _playerFactory = playerFactory;
        }

        public void Enter()
        {
            _networkManagerService.OnServerReadyEvent += CreateNewRoomPlayer;
            _networkManagerService.OnRoomClientEnterEvent += UpdatePlayersUI;
        }

        private void UpdatePlayersUI()
        {
            foreach (var player in _networkManagerService.PlayersInRoom)
            {
                if (player is RoomPlayer roomPlayer) 
                    roomPlayer.UpdatePlayerUI();
            }
        }

        public void Enter(NetworkDataPayload payload)
        {
            _networkManagerService.OnServerReadyEvent += CreateNewRoomPlayer;
            _payload = payload;
        }

        [Server]
        private void CreateNewRoomPlayer(NetworkConnectionToClient conn)
        {
            RoomPlayer player = _networkManagerService.RoomPlayerPrefab;
            _playerFactory.CreateRoomPlayer(conn, player);
        }
        
        public void Exit()
        {
            _networkManagerService.OnServerReadyEvent -= CreateNewRoomPlayer;
            _networkManagerService.OnRoomClientEnterEvent -= UpdatePlayersUI;
            _uiFactory.ClearUIRoot();
        }
    }
}
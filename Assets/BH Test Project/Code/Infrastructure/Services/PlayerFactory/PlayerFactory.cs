using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services.PlayerFactory
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly INetworkManagerService _networkManagerService;
        private readonly IUIFactory _uiFactory;
        private readonly IGameStateMachine _gameStateMachine;

        public PlayerFactory(INetworkManagerService networkManagerService, IUIFactory uiFactory,
            IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _networkManagerService = networkManagerService;
        }

        public RoomPlayer CreateRoomPlayer(NetworkConnectionToClient conn)
        {
            RoomPlayer player = Object.Instantiate(_networkManagerService.RoomPlayerPrefab);
            player.Init(_gameStateMachine, _uiFactory);
            NetworkServer.Spawn(player.gameObject, conn);
            return player;
        }
    }
}
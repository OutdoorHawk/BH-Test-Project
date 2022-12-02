using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services.PlayerFactory
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IUIFactory _uiFactory;
        private readonly IGameStateMachine _gameStateMachine;
        private IPlayerFactory _playerFactoryImplementation;

        public PlayerFactory(IUIFactory uiFactory,
            IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
        }

        public RoomPlayer CreateRoomPlayer(NetworkConnectionToClient conn, RoomPlayer roomPlayer)
        {
            RoomPlayer player = Object.Instantiate(roomPlayer);
            player.Init(_gameStateMachine, _uiFactory);
            NetworkServer.Spawn(player.gameObject, conn);
            return player;
        }
    }
}
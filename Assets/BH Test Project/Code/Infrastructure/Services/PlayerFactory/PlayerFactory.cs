using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Services.UI;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Runtime.Lobby;
using BH_Test_Project.Code.Runtime.Player;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services.PlayerFactory
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IUIFactory _uiFactory;
        private readonly IGameStateMachine _gameStateMachine;

        public PlayerFactory(IUIFactory uiFactory, IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
        }

        public RoomPlayer CreateRoomPlayer(NetworkConnectionToClient conn, RoomPlayer roomPlayer)
        {
            RoomPlayer spawnedPlayer = Object.Instantiate(roomPlayer);
            NetworkServer.AddPlayerForConnection(conn, spawnedPlayer.gameObject);
            spawnedPlayer.netIdentity.AssignClientAuthority(conn);
            spawnedPlayer.RpcConstruct();
            spawnedPlayer.RpcInitializePlayer();
            return spawnedPlayer;
        }

        public PlayerBehavior CreateGamePlayer(NetworkConnectionToClient conn, GameObject gamePlayer)
        {
            PlayerBehavior spawnedPlayer = Object.Instantiate(gamePlayer).GetComponent<PlayerBehavior>();
            NetworkServer.ReplacePlayerForConnection(conn, spawnedPlayer.gameObject);
            return spawnedPlayer;
        }
    }
}
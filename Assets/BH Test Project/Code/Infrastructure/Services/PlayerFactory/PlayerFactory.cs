using System.Collections.Generic;
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

        public PlayerFactory(IUIFactory uiFactory, IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
        }

        public RoomPlayer CreateRoomPlayer(NetworkConnectionToClient conn, RoomPlayer roomPlayer)
        {
            RoomPlayer spawnedPlayer = Object.Instantiate(roomPlayer);
            NetworkServer.AddPlayerForConnection(conn, spawnedPlayer.gameObject);
            return spawnedPlayer;
        }

        public void InitializePlayers(List<NetworkRoomPlayer> slotPlayers)
        {
            Debug.Log(slotPlayers.Count);
            foreach (var pl in slotPlayers)
            {
                if (pl.TryGetComponent(out RoomPlayer player))
                {
                    player.Construct(_gameStateMachine, _uiFactory);
                    if (!player.Initialized) 
                        player.Init();
                    player.UpdateUI();
                }
            }
        }
    }
}
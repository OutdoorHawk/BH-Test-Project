using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.Player;
using BH_Test_Project.Code.Runtime.UI;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkPlayerSystem
    {
        private readonly List<PlayerOnServer> _players;
        private readonly PlayerGameUI _playerGameUI;

        public NetworkPlayerSystem()
        {
           // _playerGameUI = playerGameUI;
           // _playerGameUI.Init();
            _players = new List<PlayerOnServer>();
        }
        
        public void RegisterHandlers() => 
            NetworkServer.RegisterHandler<PlayerHitMessage>(OnPlayerHit);

        [Server]
        private void OnPlayerHit(NetworkConnection connection, PlayerHitMessage message)
        {
            HitPlayer(message.HurtPlayerNetId);
        }

        [Server]
        private void HitPlayer(uint targetID)
        {
            Debug.Log(_players.Count);
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].NetID == targetID)
                    _players[i].playerBehavior.RpcHitPlayer();
            }
        }
        
        public void AddNewPlayer(PlayerBehavior playerBehavior, NetworkConnectionToClient conn)
        {
            PlayerOnServer playerOnServer = new PlayerOnServer(playerBehavior, conn);
            _players.Add(playerOnServer);
            //_playerGameUI.RpcAddPlayerToScoreTable(playerOnServer.Name, playerOnServer.playerBehavior.netIdentity);
        }

        public void RemovePlayer(NetworkConnectionToClient conn)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].Connection == conn)
                    _players.Remove(_players[i]);
            }
        }
    }
}
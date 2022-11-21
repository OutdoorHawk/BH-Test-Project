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

        private void OnPlayerHit(NetworkConnection connection, PlayerHitMessage message)
        {
            HitPlayer(message.HurtPlayerNetId);
        }

        [Server]
        private void HitPlayer(uint targetID)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity.netId == targetID)
                {
                    conn.identity.TryGetComponent(out PlayerBehavior playerBehavior);
                    playerBehavior.RpcHitPlayer();
                }
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
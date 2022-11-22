using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.Player;
using BH_Test_Project.Code.Runtime.Player.UI;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkPlayerSystem : NetworkBehaviour
    {
        private List<PlayerOnServer> _players = new();
        private PlayerGameUI _playerGameUI;

        public void Init(PlayerGameUI playerGameUI)
        {
            _playerGameUI = playerGameUI;
        }

        public void RegisterHandlers()
        {
            NetworkServer.RegisterHandler<PlayerHitMessage>(OnPlayerHit);
            NetworkClient.RegisterHandler<PlayerConnectedMessage>(OnPlayerConnected);
        }

        private void OnPlayerHit(NetworkConnection connection, PlayerHitMessage message)
        {
            HitPlayer(message.HurtPlayerNetId);
            IncreasePlayerScore(message.SuccessPlayerNetId);
        }

        private void OnPlayerConnected(PlayerConnectedMessage MSG)
        {
            _playerGameUI.AddPlayerToScoreTable(MSG);
            _players.Add(new PlayerOnServer(MSG.NetId));
        }

        [Server]
        private void HitPlayer(uint hurtPlayerNetId)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity.netId == hurtPlayerNetId)
                {
                    conn.identity.TryGetComponent(out PlayerBehavior playerBehavior);
                    playerBehavior.RpcHitPlayer();
                }
            }
        }

        [Server]
        private void IncreasePlayerScore(uint successPlayerNetId)
        {
            int newScore = 0;
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].NetID == successPlayerNetId)
                {
                    _players[i].IncreasePlayerScore();
                    newScore = _players[i].Score;
                }
            }
            
            foreach (var conn in NetworkServer.connections.Values)
            {
                conn.identity.TryGetComponent(out PlayerBehavior playerBehavior);
                playerBehavior.RpcIncreasePlayerScore(successPlayerNetId, newScore);
            }
        }
    }
}
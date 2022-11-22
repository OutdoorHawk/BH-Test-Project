using System;
using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.Player;
using BH_Test_Project.Code.Runtime.Player.UI;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkPlayerSystem : NetworkBehaviour
    {
        public event Action OnGameEnd;
        private List<PlayerOnServer> _players = new();
        private PlayerGameUI _playerGameUI;

        private int _gameEndScore = 3;

        public void Init(PlayerGameUI playerGameUI)
        {
            _playerGameUI = playerGameUI;
            for (int i = 0; i < _players.Count; i++)
                _players[i].ResetScore();
        }

        public void RegisterHandlers()
        {
            NetworkClient.RegisterHandler<PlayerConnectedMessage>(OnPlayerConnected);
            NetworkServer.RegisterHandler<PlayerAskHitMessage>(OnPlayerAskHit);
            NetworkClient.RegisterHandler<PlayerHitSuccessMessage>(OnPlayerHitSucceed);
        }

        public void UnregisterHandlers()
        {
            NetworkClient.UnregisterHandler<PlayerConnectedMessage>();
            NetworkServer.UnregisterHandler<PlayerAskHitMessage>();
            NetworkClient.UnregisterHandler<PlayerHitSuccessMessage>();
        }

        private void OnPlayerConnected(PlayerConnectedMessage MSG)
        {
            _playerGameUI.AddPlayerToScoreTable(MSG);
            _players.Add(new PlayerOnServer(MSG.NetId, MSG.PlayerName));
        }

        private void OnPlayerAskHit(NetworkConnection connection, PlayerAskHitMessage message)
        {
            SendPlayerHitRpc(message.HitRecipientNetId, message.HitSenderNetId);
        }

        private void SendPlayerHitRpc(uint hitRecipientNetId, uint hitSenderNetId)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity.netId == hitRecipientNetId)
                {
                    conn.identity.TryGetComponent(out PlayerBehavior playerBehavior);
                    playerBehavior.RpcHitPlayer(hitSenderNetId);
                }
            }
        }

        private void OnPlayerHitSucceed(PlayerHitSuccessMessage msg)
        {
            foreach (var player in _players.Where(player => player.NetID == msg.HitSenderNetId))
            {
                player.IncreasePlayerScore();
                UpdatePlayersScoreUI(msg.HitSenderNetId, player.Score);
                CheckGameEndConditions(player);
            }
        }

        private void UpdatePlayersScoreUI(uint successPlayerNetId, int newScore)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                conn.identity.TryGetComponent(out PlayerBehavior playerBehavior);
                playerBehavior.RpcIncreasePlayerScore(successPlayerNetId, newScore);
            }
        }

        private void CheckGameEndConditions(PlayerOnServer player)
        {
            if (player.Score == _gameEndScore)
            {
                foreach (var conn in NetworkServer.connections.Values)
                {
                    conn.identity.TryGetComponent(out PlayerBehavior playerBehavior);
                    playerBehavior.RpcGameEnd(player.Name);
                }

                CmdGameOver();
                //if (isServer)
                // OnGameEnd?.Invoke();
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdGameOver()
        {
            NetworkServer.SendToAll(new GameRestartMessage());
        }
    }
}
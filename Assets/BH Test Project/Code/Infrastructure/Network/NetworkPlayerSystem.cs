using System.Collections.Generic;
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
            //IncreasePlayerScore(message);
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
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity.netId == successPlayerNetId)
                {
                    conn.identity.TryGetComponent(out PlayerBehavior playerBehavior);
                    playerBehavior.RpcIncreasePlayerScore(successPlayerNetId);
                }
            }
        }
        
        private void OnPlayerConnected(PlayerConnectedMessage MSG)
        {
            _playerGameUI.AddPlayerToScoreTable(MSG);
        }

        public void AddNewPlayer(uint netID)
        {
            PlayerOnServer newPlayer = new PlayerOnServer(netID);
            _players.Add(newPlayer);

            PlayerConnectedMessage playerConnectedMessage = new PlayerConnectedMessage()
            {
                NetId = netID,
                PlayerName = $"{PlayerPrefs.GetString(Constants.PLAYER_NAME)}{netID}",
                Id = _players.Count
            };
            NetworkServer.SendToAll(playerConnectedMessage);
        }
    }
}
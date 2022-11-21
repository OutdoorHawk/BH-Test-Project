using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.Player;
using BH_Test_Project.Code.Runtime.Player.UI;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkPlayerSystem : NetworkBehaviour
    {
        private readonly SyncList<PlayerOnServer> _players = new();
        private PlayerGameUI _playerGameUI;

        private void Awake()
        {
            _players.Callback += OnPlayersListChanged;
        }

        public void Init(PlayerGameUI playerGameUI)
        {
            _playerGameUI = playerGameUI;
        }

        private void OnPlayersListChanged(SyncList<PlayerOnServer>.Operation op, int itemIndex,
            PlayerOnServer oldItem, PlayerOnServer newItem)
        {
            switch (op)
            {
                case SyncList<PlayerOnServer>.Operation.OP_REMOVEAT:
                    break;
                case SyncList<PlayerOnServer>.Operation.OP_SET:
                    Debug.Log("set");
                   // _playerGameUI.UpdatePlayerScore(newItem.Score, newItem.NetID);
                    break;
            }
        }

        public void RegisterHandlers()
        {
            NetworkServer.RegisterHandler<PlayerHitMessage>(OnPlayerHit);
            NetworkClient.RegisterHandler<PlayerConnectedMessage>(OnPlayerConnected);
        }

        private void OnPlayerHit(NetworkConnection connection, PlayerHitMessage message)
        {
            HitPlayer(message.HurtPlayerNetId);
            for (var i = 0; i < _players.Count; i++)
            {
                var pl = _players[i];
                if (pl.NetID == message.SuccessPlayerNetId)
                    pl.Score++;
            }
        }
        
        private void OnPlayerConnected(PlayerConnectedMessage MSG)
        {
            _playerGameUI.AddPlayerToScoreTable(MSG);
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

        public void AddNewPlayer(NetworkConnectionToClient conn)
        {
            _players.Add(new PlayerOnServer(conn.identity.netId));
            PlayerConnectedMessage playerConnectedMessage = new PlayerConnectedMessage()
            {
                NetId = conn.identity.netId,
                PlayerName = $"PLAYER{conn.identity.netId}",
                Id = _players.Count
            };
            NetworkServer.SendToAll(playerConnectedMessage);
        }
    }
}
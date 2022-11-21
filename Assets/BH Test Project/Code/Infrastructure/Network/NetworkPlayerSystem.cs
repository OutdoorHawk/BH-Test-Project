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

        public void Init(PlayerGameUI playerGameUI)
        {
            _playerGameUI = playerGameUI;
            _players.Callback += OnPlayersListChanged;
        }

        private void OnPlayersListChanged(SyncList<PlayerOnServer>.Operation op, int itemIndex,
            PlayerOnServer oldItem, PlayerOnServer newItem)
        {
            switch (op)
            {
                case SyncList<PlayerOnServer>.Operation.OP_ADD:
                    _playerGameUI.AddPlayerToScoreTable(newItem, itemIndex);
                    break;
                case SyncList<PlayerOnServer>.Operation.OP_CLEAR:
                    break;
                case SyncList<PlayerOnServer>.Operation.OP_INSERT:
                    break;
                case SyncList<PlayerOnServer>.Operation.OP_REMOVEAT:
                    break;
                case SyncList<PlayerOnServer>.Operation.OP_SET:
                    break;
            }
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
            _players.Add(new PlayerOnServer(conn.identity.netId));
        }
    }
}
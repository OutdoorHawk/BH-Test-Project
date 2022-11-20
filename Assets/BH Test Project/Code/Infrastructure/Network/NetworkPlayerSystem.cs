using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.Player;
using BH_Test_Project.Code.Runtime.UI;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkPlayerSystem
    {
        private readonly SyncList<PlayerOnServer> _players;
        private readonly PlayerGameUI _playerGameUI;

        public NetworkPlayerSystem(PlayerGameUI playerGameUI)
        {
            _playerGameUI = playerGameUI;
            _players = new SyncList<PlayerOnServer>();
            NetworkServer.RegisterHandler<PlayerHitMessage>(OnPlayerHit);
        }

        [Server]
        private void OnPlayerHit(NetworkConnection connection, PlayerHitMessage message)
        {
            HitPlayer(message.HurtPlayerNetId);
        }

        [Server]
        private void HitPlayer(uint targetID)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].NetID == targetID)
                    _players[i].Player.RpcHitPlayer();
            }
        }

        [Server]
        public void AddNewPlayer(Player player, NetworkConnectionToClient conn)
        {
            PlayerOnServer playerOnServer = new PlayerOnServer(player, conn);
            _players.Add(playerOnServer);
            _playerGameUI.RpcAddPlayerToScoreTable(playerOnServer);
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
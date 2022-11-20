using BH_Test_Project.Code.Runtime.Player;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkPlayerSystem
    {
        private readonly SyncList<PlayerOnServer> _players;

        public NetworkPlayerSystem()
        {
            _players = new SyncList<PlayerOnServer>();
        }

        public void AddNewPlayer(Player player, NetworkConnectionToClient conn)
        {
            _players.Add(new PlayerOnServer(player, conn));
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
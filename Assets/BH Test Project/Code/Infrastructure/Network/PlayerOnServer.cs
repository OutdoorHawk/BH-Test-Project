using BH_Test_Project.Code.Runtime.Player;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class PlayerOnServer
    {
        public readonly Player Player;
        public readonly uint NetID;
        public readonly NetworkConnectionToClient Connection;

        public PlayerOnServer(Player player, NetworkConnectionToClient connection)
        {
            Player = player;
            Connection = connection;
            NetID = player.netId;
        }
    }
}
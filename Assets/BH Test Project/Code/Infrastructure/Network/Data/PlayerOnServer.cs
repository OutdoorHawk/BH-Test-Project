using BH_Test_Project.Code.Runtime.Player;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class PlayerOnServer
    {
        public readonly Player Player;
        public readonly uint NetID;
        public readonly string Name;
        public readonly NetworkConnectionToClient Connection;

        public PlayerOnServer(Player player, NetworkConnectionToClient connection)
        {
            Player = player;
            Connection = connection;
            NetID = player.netId;
            Name = $"Player{NetID}";
            Score = 0;
        }

        public int Score { get; private set; }

        public void IncreasePlayerScore()
        {
            Score++;
        }

        public void ResetScore()
        {
            Score = 0;
        }
    }
}
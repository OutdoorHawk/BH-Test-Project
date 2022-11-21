using BH_Test_Project.Code.Runtime.Player;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class PlayerOnServer
    {
        public readonly PlayerBehavior playerBehavior;
        public readonly uint NetID;
        public readonly string Name;
        public readonly NetworkConnectionToClient Connection;

        public PlayerOnServer(PlayerBehavior playerBehavior, NetworkConnectionToClient connection)
        {
            this.playerBehavior = playerBehavior;
            Connection = connection;
            NetID = playerBehavior.netId;
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
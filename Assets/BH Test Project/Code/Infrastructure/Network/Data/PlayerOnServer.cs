namespace BH_Test_Project.Code.Infrastructure.Network.Data
{
    public class PlayerOnServer
    {
        public uint NetID;
        public int Score;

        public PlayerOnServer(uint netID)
        {
            NetID = netID;
            Score = 0;
        }

        public void IncreasePlayerScore()
        {
            Score += 1;
        }

        public void ResetScore()
        {
            Score = 0;
        }
    }
}
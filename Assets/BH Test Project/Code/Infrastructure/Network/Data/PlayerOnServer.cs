namespace BH_Test_Project.Code.Infrastructure.Network.Data
{
    public class PlayerOnServer
    {
        public uint NetID;
        public int Score;
        public string Name;

        public PlayerOnServer(uint netID, string playerName)
        {
            NetID = netID;
            Score = 0;
            Name = playerName;
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
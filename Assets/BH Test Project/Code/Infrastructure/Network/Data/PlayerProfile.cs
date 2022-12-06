namespace BH_Test_Project.Code.Infrastructure.Network.Data
{
    public struct PlayerProfile
    {
        public readonly string PlayerName;
        public readonly int ConnectionID;
        public int Score;

        public PlayerProfile(string playerName, int connectionID)
        {
            PlayerName = playerName;
            Score = 0;
            ConnectionID = connectionID;
        }

        public void IncreasePlayerScore()
        {
            Score++;
        }

        public void ResetPlayerScore()
        {
            Score = 0;
        }
    }
}
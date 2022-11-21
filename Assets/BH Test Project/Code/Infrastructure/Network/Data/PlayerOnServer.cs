using BH_Test_Project.Code.Infrastructure.Data;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network.Data
{
    public struct PlayerOnServer
    {
        public uint NetID;
        public string Name;
        public int Score;

        public PlayerOnServer(uint netID)
        {
            NetID = netID;
            Name = PlayerPrefs.GetString(Constants.PLAYER_NAME);
            Score = 0;
        }

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
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network.Data
{
    public struct PlayerProfile
    {
        public readonly string PlayerName;
        public readonly int Score;

        public PlayerProfile(string playerName)
        {
            PlayerName = playerName;
            Score = 0;
        }
    }
}

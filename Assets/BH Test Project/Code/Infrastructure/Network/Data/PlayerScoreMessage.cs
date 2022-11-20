using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Network.Data
{
    public struct PlayerScoreMessage : NetworkMessage
    {
        public uint NetId;
        public uint Score;
        public string Name;
    }
}
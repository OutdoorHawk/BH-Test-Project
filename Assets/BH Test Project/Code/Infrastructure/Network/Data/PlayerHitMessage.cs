using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Network.Data
{
    public struct PlayerHitMessage : NetworkMessage
    {
        public uint HurtPlayerNetId;
        public uint SuccessPlayerNetId;
    }
}
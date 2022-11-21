using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Network.Data
{
    public struct PlayerHitMessage : NetworkMessage
    {
        public uint HurtPlayerNetId;
        public uint SuccessPlayerNetId;
    } 
    
    public struct PlayerConnectedMessage : NetworkMessage
    {
        public string PlayerName;
        public uint NetId;
        public int Id;
    } 
    
}
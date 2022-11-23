using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Network.Data
{
    public struct PlayerAskHitMessage : NetworkMessage
    {
        public uint HitRecipientNetId;
        public uint HitSenderNetId;
    }

    public struct PlayerHitSuccessMessage : NetworkMessage
    {
        public uint HitSenderNetId;
    }

    public struct PlayerConnectedMessage : NetworkMessage
    {
        public string PlayerName;
        public uint NetId;
    }

    public struct GameRestartMessage : NetworkMessage
    {
    }
    
    public struct RoomPlayerAddedMessage : NetworkMessage
    {
    }
}
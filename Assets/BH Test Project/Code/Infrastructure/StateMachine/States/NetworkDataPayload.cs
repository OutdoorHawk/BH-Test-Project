using Mirror;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public struct NetworkDataPayload
    {
        public readonly bool IsServer;

        public NetworkDataPayload( bool isServer)
        {
            IsServer = isServer;
        }
    }
}
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public struct NetworkDataPayload
    {
        public readonly bool IsServer;
        public readonly NetworkConnectionToClient CurrentConnection;

        public NetworkDataPayload(NetworkConnectionToClient conn, bool isServer)
        {
            IsServer = isServer;
            CurrentConnection = conn;
        }
    }
}
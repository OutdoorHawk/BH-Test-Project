namespace MirrorServiceTest.Code.Infrastructure.StateMachine.States
{
    public struct NetworkDataPayload
    {
        public readonly bool IsHost;

        public NetworkDataPayload( bool isHost)
        {
            IsHost = isHost;
        }
    }
}
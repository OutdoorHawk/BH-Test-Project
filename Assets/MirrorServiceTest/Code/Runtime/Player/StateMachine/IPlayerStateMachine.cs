namespace MirrorServiceTest.Code.Runtime.Player.StateMachine
{
    public interface IPlayerStateMachine
    {
        ITickableState ActiveState { get; }
        void Enter<TState>() where TState : class, ITickableState;
        void Tick();
        void CleanUp();
    }
}
using MirrorServiceTest.Code.Infrastructure.StateMachine;

namespace MirrorServiceTest.Code.Runtime.Player.StateMachine
{
    public interface ITickableState : IState
    {
        void Tick();
    }
}
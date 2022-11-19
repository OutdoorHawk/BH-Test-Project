using BH_Test_Project.Code.Infrastructure.StateMachine;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine
{
    public interface ITickableState : IState
    {
        void Tick();
    }
}
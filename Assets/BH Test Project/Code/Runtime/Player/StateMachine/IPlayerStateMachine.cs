using BH_Test_Project.Code.Runtime.Player.StateMachine.States;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine
{
    public interface IPlayerStateMachine
    {
        void Enter<TState>() where TState : class, ITickableState;
        TState ChangeState<TState>() where TState : class, ITickableState;
        TState GetState<TState>() where TState : class, ITickableState;
        void Tick();
        void CleanUp();
    }
}
using BH_Test_Project.Code.Runtime.Player.StateMachine.States;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine
{
    public interface IPlayerStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        TState ChangeState<TState>() where TState : class, IState;
        TState GetState<TState>() where TState : class, IState;
        void Tick();
        void CleanUp();
    }
}
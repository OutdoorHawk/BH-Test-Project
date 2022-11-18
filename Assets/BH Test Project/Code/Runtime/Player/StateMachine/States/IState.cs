namespace BH_Test_Project.Code.Runtime.Player.StateMachine.States
{
    public interface IState
    {
        void Enter();
        void Tick();
        void Exit();
    }
}
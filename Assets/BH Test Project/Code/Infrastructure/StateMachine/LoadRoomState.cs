using BH_Test_Project.Code.Infrastructure.DI;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class LoadRoomState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;

        public LoadRoomState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
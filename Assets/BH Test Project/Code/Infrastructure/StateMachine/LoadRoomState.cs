using BH_Test_Project.Code.Infrastructure.DI;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class LoadRoomState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly DIContainer _diContainer;

        public LoadRoomState(IGameStateMachine gameStateMachine, DIContainer diContainer)
        {
            _gameStateMachine = gameStateMachine;
            _diContainer = diContainer;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
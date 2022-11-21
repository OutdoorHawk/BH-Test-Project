using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Infrastructure.Services;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class LoadOfflineMenuState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IStaticDataService _staticDataService;

        public LoadOfflineMenuState(IGameStateMachine gameStateMachine, IUIFactory uiFactory,
            IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
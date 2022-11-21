using BH_Test_Project.Code.Infrastructure.Services;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneContextService _sceneContextService;

        public GameLoopState(IGameStateMachine gameStateMachine, IStaticDataService staticDataService,
            ISceneContextService sceneContextService)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _sceneContextService = sceneContextService;
        }

        public void Enter()
        {
            //NetworkManager.singleton.st
        }

        public void Exit()
        {
        }
    }
}
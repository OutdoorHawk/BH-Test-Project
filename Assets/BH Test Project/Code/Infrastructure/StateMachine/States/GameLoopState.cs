using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Services;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneContextService _sceneContextService;
        private NetworkPlayerSystem _playerSystem;


        public GameLoopState(IGameStateMachine gameStateMachine, IStaticDataService staticDataService,
            ISceneContextService sceneContextService)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _sceneContextService = sceneContextService;
        }

        public void Enter()
        {
            _sceneContextService.InitSceneContext();
            _playerSystem = _sceneContextService.GetPlayerSystem();
            _playerSystem.RegisterHandlers();
        }

        public void Exit()
        {
            _playerSystem.UnregisterHandlers();
        }
    }
}
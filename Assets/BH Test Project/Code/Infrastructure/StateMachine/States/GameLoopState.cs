using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.StaticData;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneContextService _sceneContextService;
        private NetworkPlayerSystem _playerSystem;
        private NetworkSpawnSystem _spawnSystem;

        public GameLoopState(IGameStateMachine gameStateMachine, IStaticDataService staticDataService,
            ISceneContextService sceneContextService)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _sceneContextService = sceneContextService;
        }

        public void Enter()
        {
            InitGameLevel();
        }

        private void InitGameLevel()
        {
            _sceneContextService.InitSceneContext();
            _spawnSystem = new NetworkSpawnSystem(_sceneContextService.GetSceneSpawnPoints());
            InitPlayerSystem();
        }

        private void InitPlayerSystem()
        {
            _playerSystem = _sceneContextService.GetPlayerSystem();
            _playerSystem.RegisterHandlers();
            WorldStaticData worldStaticData = _staticDataService.GetWorldStaticData();
            PlayerStaticData playerStaticData = _staticDataService.GetPlayerStaticData();
            _playerSystem.Init(_sceneContextService.GetPlayerUI(), worldStaticData, playerStaticData);
        }

        public void Exit()
        {
            _playerSystem.UnregisterHandlers();
        }
    }
}
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Runtime.Player.UI;
using BH_Test_Project.Code.StaticData;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneContextService _sceneContextService;
        private readonly IUIFactory _uiFactory;
        private NetworkPlayerSystem _playerSystem;
        private NetworkSpawnSystem _spawnSystem;

        public GameLoopState(IGameStateMachine gameStateMachine, IStaticDataService staticDataService,
            ISceneContextService sceneContextService, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _sceneContextService = sceneContextService;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            InitGameLevel();
        }

        private void InitGameLevel()
        {
            _sceneContextService.InitSceneContext();
            _spawnSystem = new NetworkSpawnSystem(_sceneContextService.GetSceneSpawnPoints());
            NetworkManager.startPositions = _sceneContextService.GetSceneSpawnPoints();
            InitPlayerSystem();
        }

        private void InitPlayerSystem()
        {
            //_playerSystem = Object.Instantiate(_staticDataService.GetPlayerNetworkSystem());
            _playerSystem = _sceneContextService.GetPlayerSystem();
            // NetworkManager.singleton.spawnPrefabs.Add(_playerSystem.gameObject);
            //  NetworkServer.Spawn(_playerSystem.gameObject);
            _playerSystem.RegisterHandlers();
            WorldStaticData worldStaticData = _staticDataService.GetWorldStaticData();
            PlayerStaticData playerStaticData = _staticDataService.GetPlayerStaticData();
            PlayerHUD playerHUD = _uiFactory.CreatePlayerHUD();
            _playerSystem.Init(playerHUD, worldStaticData, playerStaticData);
        }

        public void Exit()
        {
            _uiFactory.ClearUIRoot();
            _playerSystem.UnregisterHandlers();
        }
    }
}
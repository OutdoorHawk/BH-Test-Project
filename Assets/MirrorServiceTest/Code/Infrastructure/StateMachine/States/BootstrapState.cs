using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Infrastructure.Services.CoroutineRunner;
using MirrorServiceTest.Code.Infrastructure.Services.Network;
using MirrorServiceTest.Code.Infrastructure.Services.PlayerFactory;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService;
using MirrorServiceTest.Code.Infrastructure.Services.SceneContext;
using MirrorServiceTest.Code.Infrastructure.Services.SceneLoaderService;
using MirrorServiceTest.Code.Infrastructure.Services.StaticData;
using MirrorServiceTest.Code.Infrastructure.Services.UI;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly DIContainer _diContainer;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly RecordingService _recordingService;
        private IUIFactory _uiFactory;

        private GameNetworkService _gameNetworkService;

        public BootstrapState(IGameStateMachine gameStateMachine, DIContainer diContainer,
            ICoroutineRunner coroutineRunner, RecordingService recordingService)
        {
            _recordingService = recordingService;
            _coroutineRunner = coroutineRunner;
            _gameStateMachine = gameStateMachine;
            _diContainer = diContainer;
            BindServices();
        }

        private void BindServices()
        {
            _diContainer.BindSingle(_gameStateMachine);
            _diContainer.BindSingle<ISceneContextService>(new SceneContextService());
            BindStaticDataService();
            BindFactories();
            BindNetworkManagerService();
            _diContainer.BindSingle(_coroutineRunner);
            _diContainer.BindSingle<ISceneLoader>(new SceneLoader(_coroutineRunner));
            _diContainer.BindSingle<IRecordingService>(_recordingService);
        }

        private void BindFactories()
        {
            _diContainer.BindSingle<IUIFactory>(new UIFactory(_diContainer.Resolve<IStaticDataService>()));
            _diContainer.BindSingle<IPlayerFactory>(new PlayerFactory(_diContainer.Resolve<IStaticDataService>(),
                _diContainer.Resolve<ISceneContextService>(),_diContainer.Resolve<IRecordingService>()));
        }

        private void BindStaticDataService()
        {
            IStaticDataService staticDataService = new StaticDataService();
            staticDataService.Load();
            _diContainer.BindSingle(staticDataService);
        }

        private void BindNetworkManagerService()
        {
            var staticDataService = _diContainer.Resolve<IStaticDataService>();
            _gameNetworkService = Object.Instantiate(staticDataService.GetLobbyNetworkManager());
            _gameNetworkService.Init(_diContainer.Resolve<IPlayerFactory>(),
                staticDataService.GetWorldStaticData());
            _diContainer.BindSingle<IGameNetworkService>(_gameNetworkService);
        }

        public void Enter()
        {
            _uiFactory = _diContainer.Resolve<IUIFactory>();
            _uiFactory.CreateUiRoot();
            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit()
        {
        }
    }
}
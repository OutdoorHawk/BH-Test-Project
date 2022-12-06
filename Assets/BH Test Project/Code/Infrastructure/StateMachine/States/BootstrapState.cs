using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.Services.CoroutineRunner;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.PlayerFactory;
using BH_Test_Project.Code.Infrastructure.Services.SceneContext;
using BH_Test_Project.Code.Infrastructure.Services.SceneLoaderService;
using BH_Test_Project.Code.Infrastructure.Services.StaticData;
using BH_Test_Project.Code.Infrastructure.Services.UI;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly DIContainer _diContainer;
        private readonly ICoroutineRunner _coroutineRunner;
        private IUIFactory _uiFactory;

        private GameNetworkService _gameNetworkService;

        public BootstrapState(IGameStateMachine gameStateMachine, DIContainer diContainer,
            ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _gameStateMachine = gameStateMachine;
            _diContainer = diContainer;
            BindServices();
        }

        private void BindServices()
        {
            _diContainer.BindSingle(_gameStateMachine);
            BindStaticDataService();
            BindFactories();
            BindNetworkManagerService();
            _diContainer.BindSingle<ISceneContextService>(new SceneContextService());
            _diContainer.BindSingle(_coroutineRunner);
            _diContainer.BindSingle<ISceneLoader>(new SceneLoader(_coroutineRunner));
        }

        private void BindFactories()
        {
            _diContainer.BindSingle<IUIFactory>(new UIFactory(_diContainer.Resolve<IStaticDataService>()));
            _diContainer.BindSingle<IPlayerFactory>(new PlayerFactory(_diContainer.Resolve<IUIFactory>(),
                _diContainer.Resolve<IGameStateMachine>(), _diContainer.Resolve<IStaticDataService>()));
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
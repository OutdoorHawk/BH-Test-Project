using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Infrastructure.Services;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly DIContainer _diContainer;
        private IUIFactory _uiFactory;

        public BootstrapState(IGameStateMachine gameStateMachine, DIContainer diContainer)
        {
            _gameStateMachine = gameStateMachine;
            _diContainer = diContainer;
            BindServices();
        }

        private void BindServices()
        {
            BindStaticDataService();
            _diContainer.BindSingle(_gameStateMachine);
            _diContainer.BindSingle<ISceneContextService>(new SceneContextService());
            _diContainer.BindSingle<IUIFactory>(new UIFactory(_diContainer.Resolve<IStaticDataService>()));
        }

        private void BindStaticDataService()
        {
            IStaticDataService staticDataService = new StaticDataService();
            staticDataService.Load();
            _diContainer.BindSingle(staticDataService);
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
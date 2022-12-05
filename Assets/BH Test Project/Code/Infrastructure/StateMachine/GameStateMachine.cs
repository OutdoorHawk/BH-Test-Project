using System;
using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.Services.CoroutineRunner;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.PlayerFactory;
using BH_Test_Project.Code.Infrastructure.Services.SceneLoaderService;
using BH_Test_Project.Code.Infrastructure.Services.UI;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;

        public GameStateMachine(DIContainer diContainer, ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, diContainer, coroutineRunner),
                [typeof(MainMenuState)] = new MainMenuState(this,diContainer.Resolve<IUIFactory>(),
                    diContainer.Resolve<INetworkManagerService>(), diContainer.Resolve<ISceneLoader>()),
                [typeof(LobbyState)] = new LobbyState(this,diContainer.Resolve<IUIFactory>(),
                    diContainer.Resolve<INetworkManagerService>(), diContainer.Resolve<IPlayerFactory>()),
                [typeof(GameLoopState)] = new GameLoopState(diContainer.Resolve<IStaticDataService>(),
                    diContainer.Resolve<ISceneContextService>(), diContainer.Resolve<IUIFactory>(),
                    diContainer.Resolve<ISceneLoader>())
            };
        }

        public IExitableState ActiveState { get; private set; }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            ActiveState?.Exit();

            var state = GetState<TState>();
            ActiveState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }

        public void CleanUp()
        {
            ActiveState = null;
            _states.Clear();
        }
    }
}
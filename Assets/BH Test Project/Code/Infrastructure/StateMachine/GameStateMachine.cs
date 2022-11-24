using System;
using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;

namespace BH_Test_Project.Code.Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;

        public GameStateMachine(DIContainer diContainer)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, diContainer),
                [typeof(MainMenuState)] = new MainMenuState(this, diContainer.Resolve<IUIFactory>(),
                    diContainer.Resolve<IStaticDataService>()),
                [typeof(LobbyState)] = new LobbyState(diContainer.Resolve<IUIFactory>()),
                [typeof(GameLoopState)] = new GameLoopState(diContainer.Resolve<IStaticDataService>(),
                    diContainer.Resolve<ISceneContextService>(), diContainer.Resolve<IUIFactory>())
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
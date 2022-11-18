using System;
using System.Collections.Generic;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.StateMachine.States;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine
{
    public class PlayerStateMachine : IPlayerStateMachine
    {
        private readonly IPlayerInput _playerInput;
        private readonly Dictionary<Type, IState> _states;

        #region Init

        public PlayerStateMachine(IPlayerInput playerInput)
        {
            _playerInput = playerInput;
            _states = new Dictionary<Type, IState>
            {
                [typeof(BasicMovementState)] = new BasicMovementState(_playerInput),
                [typeof(DashState)] = new DashState(_playerInput)
            };
        }

        public IState ActiveState { get; private set; }

        #endregion

        #region Suscribe/Unsubscribe

        #endregion

        #region StateMachine

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public TState ChangeState<TState>() where TState : class, IState
        {
            ActiveState?.Exit();

            var state = GetState<TState>();
            ActiveState = state;

            return state;
        }

        public TState GetState<TState>() where TState : class, IState
        {
            return _states[typeof(TState)] as TState;
        }

        public void Tick()
        {
            ActiveState?.Tick();
        }

        #endregion

        public void CleanUp()
        {
            ActiveState = null;
            _states.Clear();
        }
    }
}
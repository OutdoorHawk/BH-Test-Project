using System;
using System.Collections.Generic;
using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.Movement;
using BH_Test_Project.Code.Runtime.Player.StateMachine.States;
using IState = BH_Test_Project.Code.Infrastructure.StateMachine.IState;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine
{
    public class PlayerStateMachine : IPlayerStateMachine
    {
        private readonly Dictionary<Type, ITickableState> _states;
        

        public PlayerStateMachine(PlayerMovement playerMovement, PlayerInput playerInput,
            PlayerAnimator playerAnimator)
        {
            _states = new Dictionary<Type, ITickableState>
            {
                [typeof(BasicMovementState)] = new BasicMovementState(this, playerMovement, playerInput),
                [typeof(DashState)] = new DashState(this, playerMovement, playerAnimator)
            };
        }

        public ITickableState ActiveState { get; private set; }
        

        public void Enter<TState>() where TState : class, ITickableState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public TState ChangeState<TState>() where TState : class, ITickableState
        {
            ActiveState?.Exit();

            var state = GetState<TState>();
            ActiveState = state;

            return state;
        }

        public TState GetState<TState>() where TState : class, ITickableState
        {
            return _states[typeof(TState)] as TState;
        }

        public void Tick()
        {
            ActiveState?.Tick();
        }
        

        public void CleanUp()
        {
            ActiveState = null;
            _states.Clear();
        }
    }
}
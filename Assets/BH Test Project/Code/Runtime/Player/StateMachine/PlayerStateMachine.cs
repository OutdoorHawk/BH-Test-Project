using System;
using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.StateMachine.States;
using BH_Test_Project.Code.Runtime.Player.Systems;
using BH_Test_Project.Code.StaticData;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine
{
    public class PlayerStateMachine : IPlayerStateMachine
    {
        private readonly Dictionary<Type, ITickableState> _states;

        public PlayerStateMachine(PlayerMovement playerMovement, IPlayerInput playerInput,
            PlayerAnimator playerAnimator, PlayerCollisionDetector playerCollisionDetector,
            PlayerGameStatus playerGameStatus, MonoBehaviour mono, PlayerStaticData playerStaticData)
        {
            _states = new Dictionary<Type, ITickableState>
            {
                [typeof(BasicMovementState)] =
                    new BasicMovementState(this, playerMovement, playerAnimator, playerInput),
                [typeof(DashState)] = new DashState(this, playerMovement, playerAnimator, playerCollisionDetector,playerGameStatus,
                    mono, playerInput, playerStaticData.DashRechargeTime),
                [typeof(EndGameState)] = new EndGameState(this, playerInput, playerMovement, playerAnimator)
            };
        }

        public ITickableState ActiveState { get; private set; }

        public void Enter<TState>() where TState : class, ITickableState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, ITickableState
        {
            ActiveState?.Exit();

            var state = GetState<TState>();
            ActiveState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, ITickableState
        {
            return _states[typeof(TState)] as TState;
        }

        public void Tick()
        {
            ActiveState?.Tick();
        }

        public void CleanUp()
        {
            ActiveState.Exit();
            ActiveState = null;
            _states.Clear();
        }
    }
}
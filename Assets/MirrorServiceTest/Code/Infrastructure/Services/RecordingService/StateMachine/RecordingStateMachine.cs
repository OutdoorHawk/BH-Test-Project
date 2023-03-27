using System;
using System.Collections.Generic;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService.Systems;
using MirrorServiceTest.Code.Infrastructure.StateMachine;
using MirrorServiceTest.Code.Runtime.Player.StateMachine;
using MirrorServiceTest.Code.Runtime.Player.UI.TimeControlMenu;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService.StateMachine
{
    public class RecordingStateMachine
    {
        private readonly Dictionary<Type, ITickableState> _states;

        public RecordingStateMachine(PlayerRecordSystem playerRecordSystem, Dictionary<long, FrameRecord> frameRecords,
            TimeControlHUD timeControlHUD)
        {
            _states = new Dictionary<Type, ITickableState>
            {
                [typeof(SaveRecordState)] = new SaveRecordState(this, playerRecordSystem, frameRecords, timeControlHUD),
                [typeof(LoadRecordState)] = new LoadRecordState(this, playerRecordSystem, frameRecords, timeControlHUD),
                [typeof(NoRecordState)] = new NoRecordState(this, timeControlHUD),
            };
        }

        public ITickableState ActiveState { get; private set; }

        public void Enter<TState>() where TState : class, ITickableState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Tick()
        {
            ActiveState?.Tick();
        }

        public void FixedTick()
        {
            ActiveState?.FixedTick();
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

        public void CleanUp()
        {
            ActiveState.Exit();
            ActiveState = null;
            _states.Clear();
        }
    }
}
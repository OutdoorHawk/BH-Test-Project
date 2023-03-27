using System.Collections.Generic;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService.Systems;
using MirrorServiceTest.Code.Runtime.Player.StateMachine;
using MirrorServiceTest.Code.Runtime.Player.UI.TimeControlMenu;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService.StateMachine
{
    public class LoadRecordState : ITickableState
    {
        private readonly RecordingStateMachine _recordingStateMachine;
        private readonly PlayerRecordSystem _playerRecordSystem;
        private readonly Dictionary<long, FrameRecord> _frameRecords;
        private readonly TimeControlHUD _timeControlHUD;

        public LoadRecordState(RecordingStateMachine recordingStateMachine, PlayerRecordSystem playerRecordSystem,
            Dictionary<long, FrameRecord> frameRecords, TimeControlHUD timeControlHUD)
        {
            _recordingStateMachine = recordingStateMachine;
            _playerRecordSystem = playerRecordSystem;
            _frameRecords = frameRecords;
            _timeControlHUD = timeControlHUD;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
        }
    }
}
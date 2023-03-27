using System.Collections.Generic;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService.Systems;
using MirrorServiceTest.Code.Runtime.Player.StateMachine;
using MirrorServiceTest.Code.Runtime.Player.UI.TimeControlMenu;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService.StateMachine
{
    public class RecordState : ITickableState
    {
        private readonly Dictionary<long, FrameRecord> _history;
        private readonly RecordingStateMachine _recordingStateMachine;
        private readonly PlayerRecordSystem _playerRecordSystem;
        private readonly TimeControlHUD _timeControlHUD;
        private readonly Slider _timeLineSlider;

        private long _lastRecordedFrame;

        public RecordState(RecordingStateMachine recordingStateMachine, PlayerRecordSystem playerRecordSystem,
            Dictionary<long, FrameRecord> history,
            TimeControlHUD timeControlHUD)
        {
            _recordingStateMachine = recordingStateMachine;
            _playerRecordSystem = playerRecordSystem;
            _history = history;
            _timeControlHUD = timeControlHUD;
            _timeLineSlider = timeControlHUD.Slider;
            _timeLineSlider.minValue = 1;
        }

        public void Enter()
        {
            _timeLineSlider.onValueChanged.AddListener(HandleSlider);
        }

        private void HandleSlider(float newValue)
        {
            
        }

        public void FixedTick()
        {
            SaveCurrentFrame();
            _timeLineSlider.maxValue = _lastRecordedFrame;
            _timeLineSlider.SetValueWithoutNotify(_lastRecordedFrame);
            _lastRecordedFrame++;
        }

        public void Tick()
        {
        }

        private void SaveCurrentFrame()
        {
            FrameRecord frameRecord = new FrameRecord();
            _playerRecordSystem.RecordPlayersData(frameRecord);
            _history.Add(_lastRecordedFrame, frameRecord);
        }

        public void Exit()
        {
            _timeLineSlider.onValueChanged.RemoveListener(HandleSlider);
        }
    }
}
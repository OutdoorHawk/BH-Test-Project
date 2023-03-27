using System.Collections.Generic;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService.Systems;
using MirrorServiceTest.Code.Runtime.Player.StateMachine;
using MirrorServiceTest.Code.Runtime.Player.UI.TimeControlMenu;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService.StateMachine
{
    public class LoadRecordState : ITickableState
    {
        private readonly Dictionary<long, FrameRecord> _history;
        private readonly RecordingStateMachine _recordingStateMachine;
        private readonly PlayerRecordSystem _playerRecordSystem;
        private readonly TimeControlHUD _timeControlHUD;
        private readonly Slider _timeLineSlider;
        
        private long _currentFrame;

        public LoadRecordState(RecordingStateMachine recordingStateMachine, PlayerRecordSystem playerRecordSystem,
            Dictionary<long, FrameRecord> history, TimeControlHUD timeControlHUD)
        {
            _recordingStateMachine = recordingStateMachine;
            _playerRecordSystem = playerRecordSystem;
            _history = history;
            _timeControlHUD = timeControlHUD;
            _timeLineSlider = timeControlHUD.Slider;
        }

        public void Enter()
        {
            _timeControlHUD.OnPlayPressed += ResumeRecord;
            _timeLineSlider.onValueChanged.AddListener(HandleSlider);
            _currentFrame = (long)_timeLineSlider.value;
        }

        private void ResumeRecord()
        {
            _recordingStateMachine.Enter<SaveRecordState>();
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
        }

        private void HandleSlider(float sliderValue)
        {
            LoadFrameData((long)sliderValue);
        }

        private void LoadFrameData(long frame)
        {
            if (_history.TryGetValue(frame, out FrameRecord frameData))
                _playerRecordSystem.LoadPlayersData(frameData);
        }

        public void Exit()
        {
            _timeControlHUD.OnPlayPressed -= ResumeRecord;
            LoadFrameData(_currentFrame);
        }
    }
}
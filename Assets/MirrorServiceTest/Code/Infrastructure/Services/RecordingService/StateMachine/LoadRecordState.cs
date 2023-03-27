using System.Collections;
using System.Collections.Generic;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService.Systems;
using MirrorServiceTest.Code.Runtime.Player.StateMachine;
using MirrorServiceTest.Code.Runtime.Player.UI.TimeControlMenu;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService.StateMachine
{
    public class LoadRecordState : ITickableState
    {
        private readonly List<KeyValuePair<long, FrameRecord>> _history;
        private readonly RecordingStateMachine _recordingStateMachine;
        private readonly PlayerRecordSystem _playerRecordSystem;
        private readonly TimeControlHUD _timeControlHUD;
        private readonly Slider _timeLineSlider;
        private readonly MonoBehaviour _mono;
        private Coroutine _backwardsRoutine;

        private long _currentFrame;

        public LoadRecordState(RecordingStateMachine recordingStateMachine, PlayerRecordSystem playerRecordSystem,
            List<KeyValuePair<long, FrameRecord>> history, TimeControlHUD timeControlHUD, MonoBehaviour mono)
        {
            _mono = mono;
            _recordingStateMachine = recordingStateMachine;
            _playerRecordSystem = playerRecordSystem;
            _history = history;
            _timeControlHUD = timeControlHUD;
            _timeLineSlider = timeControlHUD.Slider;
        }

        public void Enter()
        {
            _timeControlHUD.OnPlayPressed += ResumeRecord;
            _timeControlHUD.OnBackwardPressed += StartPlayingBackwards;
            _timeLineSlider.onValueChanged.AddListener(HandleSlider);
            _currentFrame = (long)_timeLineSlider.value;
        }

        private void ResumeRecord()
        {
            if (_backwardsRoutine != null)
            {
                _mono.StopCoroutine(_backwardsRoutine);
                _backwardsRoutine = null;
            }
            _recordingStateMachine.Enter<SaveRecordState>();
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
        }

        private void StartPlayingBackwards()
        {
            _backwardsRoutine ??= _mono.StartCoroutine(BackwardsRoutine());
        }

        private IEnumerator BackwardsRoutine()
        {
            for (int i = _history.Count - 1; i >= 0; i--)
            {
                LoadFrameData(_history[i].Key);
                yield return new WaitForFixedUpdate();
            }
        }

        private void HandleSlider(float sliderValue)
        {
            LoadFrameData((long)sliderValue);
        }

        private void LoadFrameData(long frame)
        {
            if (TryGetValue(frame, out FrameRecord frameData))
                _playerRecordSystem.LoadPlayersData(frameData);
        }

        private bool TryGetValue(long frame, out FrameRecord frameRecord)
        {
            frameRecord = null;
            foreach (var keyValue in _history)
            {
                if (keyValue.Key != frame)
                    continue;
                frameRecord = keyValue.Value;
                return true;
            }

            return false;
        }

        public void Exit()
        {
            _timeControlHUD.OnPlayPressed -= ResumeRecord;
            LoadFrameData(_currentFrame);
        }
    }
}
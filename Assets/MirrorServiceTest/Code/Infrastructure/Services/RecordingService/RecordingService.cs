using System.Collections.Generic;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService.Systems;
using MirrorServiceTest.Code.Runtime.Player;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService
{
    public class RecordingService : MonoBehaviour, IRecordingService
    {
        private readonly Dictionary<long, FrameRecord> _history = new();

        private PlayerRecordSystem _playerRecordSystem;
        private Slider _timelineSlider;
        private long _currentFrame;

        public void Initialize()
        {
            _currentFrame = 1;
        }

        public void AddPlayerToRecord(PlayerBehavior playerBehavior)
        {
            _playerRecordSystem.AddPlayer(playerBehavior);
        }

        public void SetSlider(Slider timelineSlider)
        {
            _timelineSlider = timelineSlider;
            _timelineSlider.minValue = 1;
            _timelineSlider.onValueChanged.AddListener(HandleSlider);
        }

        private void FixedUpdate()
        {
            if (Time.timeScale == 0)
                return;
            if (!AllDataInitialized())
                return;

            SaveCurrentFrame();
            _timelineSlider.maxValue = _currentFrame;
            _timelineSlider.SetValueWithoutNotify(_currentFrame);
            _currentFrame++;
        }

        private bool AllDataInitialized()
        {
            return _currentFrame != 0 && _timelineSlider != null;
        }

        private void SaveCurrentFrame()
        {
            FrameRecord frameRecord = new FrameRecord();
            _playerRecordSystem.RecordPlayersData(frameRecord);
            _history.Add(_currentFrame, frameRecord);
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

        public void CleanUp()
        {
            _history.Clear();
            _timelineSlider.onValueChanged.RemoveListener(HandleSlider);
        }
    }
}
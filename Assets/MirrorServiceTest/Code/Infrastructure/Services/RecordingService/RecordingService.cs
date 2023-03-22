using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService
{
    public class RecordingService : MonoBehaviour, IRecordingService
    {
        //private readonly List<KeyValuePair<long, FrameData>> _history = new();
        private readonly Dictionary<long, FrameData> _history = new();

        private Transform _playerTransform;
        private Slider _timelineSlider;
        private long _currentFrame;


        public void Initialize()
        {
            _currentFrame = 1;
        }

        public void SetPlayerRecording(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        public void SetSlider(Slider timelineSlider)
        {
            _timelineSlider = timelineSlider;
            _timelineSlider.onValueChanged.AddListener(HandleSlider);
            _timelineSlider.minValue = 1;
        }

        private void FixedUpdate()
        {
            if (_currentFrame == 0)
                return;
            SaveCurrentFrame();
            _timelineSlider.maxValue = _currentFrame;
            _currentFrame++;
        }

        private void SaveCurrentFrame()
        {
            FrameData frameData = new FrameData
            {
                Position = _playerTransform.position
            };
            _history.Add(_currentFrame, frameData);
        }

        private void HandleSlider(float sliderValue)
        {
            LoadFrameData((long)sliderValue);
        }

        private void LoadFrameData(long frame)
        {
            if (_history.TryGetValue(frame, out FrameData frameData))
                _playerTransform.position = frameData.Position;
        }

        private void WriteAnimation()
        {
        }

        public void CleanUp()
        {
            _history.Clear();
            _timelineSlider.onValueChanged.RemoveListener(HandleSlider);
        }
    }
}
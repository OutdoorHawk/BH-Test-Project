using System.Collections.Generic;
using Mirror;
using MirrorServiceTest.Code.Runtime.Player;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService
{
    public class RecordingService : MonoBehaviour, IRecordingService
    {
        //private readonly List<KeyValuePair<long, FrameData>> _history = new();
        private readonly Dictionary<long, FrameData> _history = new();

        private PlayerBehavior _playerBehavior;
        private Slider _timelineSlider;
        private long _currentFrame;

        public void Initialize()
        {
            _currentFrame = 1;
        }

        public void SetPlayerRecording(PlayerBehavior playerTransform)
        {
            _playerBehavior = playerTransform;
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
            return _currentFrame != 0 && _playerBehavior != null && _timelineSlider != null;
        }

        private void SaveCurrentFrame()
        {
            FrameData frameData = new FrameData
            {
                Position = _playerBehavior.transform.position
            };
            _history.Add(_currentFrame, frameData);
        }

        private void HandleSlider(float sliderValue)
        {
            LoadFrameData((long)sliderValue);
        }

        private void LoadFrameData(long frame)
        {
           // if (_history.TryGetValue(frame, out FrameData frameData))
              //  _playerBehavior.CmdSetPlayerPosition(frameData);
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
using System.Collections.Generic;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService
{
    public class RecordingService : MonoBehaviour, IRecordingService
    {
        //private readonly List<KeyValuePair<long, FrameData>> _history = new();
        private readonly Dictionary<long, FrameData> _history = new();

        private Transform _playerTransform;
        private long _currentFrame;


        public void Initialize()
        {
            _currentFrame = 1;
        }

        public void SetPlayerRecording(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        public void LoadFrameData(long frame)
        {
            if (_history.TryGetValue(frame, out FrameData frameData))
            {
                _playerTransform.position = frameData.Position;
            }
        }

        private void FixedUpdate()
        {
            if (_currentFrame == 0)
                return;
            SaveCurrentFrame();
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

        private void WriteAnimation()
        {
        }

        public void CleanUp()
        {
            _history.Clear();
        }
    }
}
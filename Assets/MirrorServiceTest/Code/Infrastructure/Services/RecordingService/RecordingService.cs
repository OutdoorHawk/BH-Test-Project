using System.Collections.Generic;
using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService.StateMachine;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService.Systems;
using MirrorServiceTest.Code.Infrastructure.Services.TimeControlService;
using MirrorServiceTest.Code.Runtime.Player;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService
{
    public class RecordingService : MonoBehaviour, IRecordingService
    {
        private readonly Dictionary<long, FrameRecord> _history = new();
        private readonly List<PlayerBehavior> _players = new();

        private readonly PlayerRecordSystem _playerRecordSystem = new();

        private RecordingStateMachine _recordingStateMachine;
        private TimeService _timeService;
        private Slider _timelineSlider;
        private long _lastRecordedFrame;

        public void Initialize()
        {
            _timeService = DIContainer.Container.Resolve<TimeService>();
            _lastRecordedFrame = 1;
            foreach (PlayerBehavior player in _players)
                _playerRecordSystem.AddPlayer(player);
            _recordingStateMachine = new RecordingStateMachine(_playerRecordSystem, _history, _players[0].TimeControl);
            _recordingStateMachine.Enter<RecordState>();
        }

        public void AddPlayerToRecord(PlayerBehavior playerBehavior)
        {
            _players.Add(playerBehavior);
        }

        private void FixedUpdate()
        {
            if (_timeService.IsPaused)
                return;
            if (!AllDataInitialized())
                return;
            _recordingStateMachine?.FixedTick();
        }

        private bool AllDataInitialized()
        {
            return _lastRecordedFrame != 0;
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
        }
    }
}
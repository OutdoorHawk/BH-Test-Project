using System;
using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Infrastructure.Services.RecordingService;
using MirrorServiceTest.Code.Infrastructure.Services.TimeControlService;
using MirrorServiceTest.Code.Runtime.Player.Input;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Runtime.Player.UI.TimeControlMenu
{
    public class TimeControlHUD : MonoBehaviour
    {
        public event Action OnPausePressed;
        public event Action OnPlayPressed;
        public event Action OnBackwardPressed;
        
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _backwardButton;
        [SerializeField] private Slider _slider;

        private TimeService _timeService;

        public Slider Slider => _slider;

        private void Awake()
        {
            _timeService = DIContainer.Container.Resolve<TimeService>();
            _pauseButton.onClick.AddListener(EnablePause);
            _playButton.onClick.AddListener(DisablePause);
            _backwardButton.onClick.AddListener(NotifyBackwardsPressed);
            _playButton.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        public void Init(PlayerInput playerInput)
        {
            playerInput.OnTabPressed += SwitchWindow;
        }

        private void SwitchWindow()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        private void EnablePause()
        {
            if (!_timeService.IsPaused)
                _timeService.EnablePause();
            _pauseButton.gameObject.SetActive(false);
            _playButton.gameObject.SetActive(true);
            OnPausePressed?.Invoke();
        }

        private void DisablePause()
        {
            if (_timeService.IsPaused) 
                _timeService.DisablePause();
            _playButton.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(true);
            OnPlayPressed?.Invoke();
        }

        private void NotifyBackwardsPressed()
        {
            OnBackwardPressed?.Invoke();
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(EnablePause);
            _playButton.onClick.RemoveListener(EnablePause);
        }
    }
}
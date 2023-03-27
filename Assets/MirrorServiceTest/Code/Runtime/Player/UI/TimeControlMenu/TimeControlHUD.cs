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
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Slider _slider;

        private TimeService _timeService;
        private IRecordingService _recordService;

        private void Awake()
        {
            _timeService = DIContainer.Container.Resolve<TimeService>();
            _recordService = DIContainer.Container.Resolve<IRecordingService>();
            _pauseButton.onClick.AddListener(ChangePauseSettings);
            gameObject.SetActive(false);
        }

        public void Init(PlayerInput playerInput)
        {
            playerInput.OnTabPressed += SwitchWindow;
            _recordService.SetSlider(_slider);
        }

        private void SwitchWindow()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        private void ChangePauseSettings()
        {
            if (!_timeService.IsPaused)
                _timeService.EnablePause();
            else
                _timeService.DisablePause();
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(ChangePauseSettings);
        }
    }
}
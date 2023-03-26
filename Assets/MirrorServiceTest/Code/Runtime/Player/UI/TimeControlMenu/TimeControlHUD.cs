using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Infrastructure.Services.TimeControlService;
using MirrorServiceTest.Code.Runtime.Player.Input;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Runtime.Player.UI.TimeControlMenu
{
    public class TimeControlHUD : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        private TimeService _timeService;

        private void Awake()
        {
            _timeService = DIContainer.Container.Resolve<TimeService>();
            _pauseButton.onClick.AddListener(ChangePauseSettings);
        }

        public void Init(PlayerInput playerInput)
        {
            playerInput.OnTabPressed += SwitchWindow;
        }

        private void SwitchWindow()
        {
            gameObject.SetActive(gameObject.activeSelf);
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
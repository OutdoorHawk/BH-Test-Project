using System;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Runtime.MainMenu.Windows
{
    public class EnterIpView : MonoBehaviour
    {
        public event Action<string> OnJoinGamePressed;

        [SerializeField] private InputField _inputField;
        [SerializeField] private Button _joinGameButton;
        [SerializeField] private Button _backButton;

        private void OnEnable()
        {
            _joinGameButton.onClick.AddListener(JoinGameButtonPressed);
            _backButton.onClick.AddListener(CloseWindow);
        }

        private void OnDisable()
        {
            _joinGameButton.onClick.RemoveListener(JoinGameButtonPressed);
            _backButton.onClick.RemoveListener(CloseWindow);
        }

        public void OpenWindow()
        {
            gameObject.SetActive(true);
        }

        private void CloseWindow()
        {
            gameObject.SetActive(false);
        }

        private void JoinGameButtonPressed()
        {
            OnJoinGamePressed?.Invoke(_inputField.text);
            CloseWindow();
        }
    }
}
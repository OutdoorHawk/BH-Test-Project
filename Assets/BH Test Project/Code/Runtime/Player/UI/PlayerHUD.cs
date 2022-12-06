using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.Systems;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.Player.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        public event Action OnDisconnectButtonPressed;
        
        [SerializeField] private Transform _layoutParent;
        [SerializeField] private GameObject _endGamePlate;
        [SerializeField] private Text _winPlayerText;
        [SerializeField] private Text _countDownText;
        [SerializeField] private Button _disconnectButton;

        private List<ScoreElement> _scoreElements = new();
        private PlayerInput _playerInput;

        private float _restartDelay;

        public void Init(float gameRestartDelay, PlayerInput playerInput)
        {
            _playerInput = playerInput;
            _scoreElements = _layoutParent.GetComponentsInChildren<ScoreElement>(true).ToList();
            _restartDelay = gameRestartDelay;
            _disconnectButton.gameObject.SetActive(false);
            _disconnectButton.onClick.AddListener(Disconnect);
            _playerInput.OnEscapePressed += SwitchDisconnectButton;
        }

        public void UpdateScoreTable(List<PlayerProfile> profiles)
        {
            ClearScoreTable();
            for (var i = 0; i < profiles.Count; i++) 
                _scoreElements[i].ActivateElement(profiles[i].PlayerName, profiles[i].Score);
        }

        private void ClearScoreTable()
        {
            foreach (var element in _scoreElements)
                element.DeactivateElement();
        }
        
        public void EnableEndGamePanel(string winnerName)
        {
            _endGamePlate.gameObject.SetActive(true);
            _winPlayerText.text = $"Winner: {winnerName}";
            StartCoroutine(EndGameTimerRoutine());
        }

        private IEnumerator EndGameTimerRoutine()
        {
            float countdown = _restartDelay;
            do
            {
                _countDownText.text = Mathf.Round(countdown).ToString("0");
                countdown -= 1;
                yield return new WaitForSeconds(1);
            } while (countdown > 0);
        }

        public void SwitchDisconnectButton()
        {
            _disconnectButton.gameObject.SetActive(!_disconnectButton.IsActive());
        }

        private void Disconnect()
        {
            OnDisconnectButtonPressed?.Invoke();
        }

        private void OnDestroy()
        {
            _playerInput.OnEscapePressed -= SwitchDisconnectButton;
            _disconnectButton.onClick.RemoveListener(Disconnect);
        }
    }
}
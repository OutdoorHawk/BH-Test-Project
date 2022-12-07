using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MirrorServiceTest.Code.Infrastructure.Data;
using MirrorServiceTest.Code.Runtime.Player.Input;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Runtime.Player.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        public event Action OnDisconnectButtonPressed;
        
        [SerializeField] private Transform _layoutParent;
        [SerializeField] private GameObject _endGamePlate;
        [SerializeField] private Text _winPlayerText;
        [SerializeField] private Text _countDownText;
        [SerializeField] private GameObject _disconnectPanel;
        private Button _disconnectButton;

        private List<ScoreElement> _scoreElements = new();
        private PlayerInput _playerInput;

        private float _restartDelay;

        public void Init(float gameRestartDelay, PlayerInput playerInput)
        {
            _playerInput = playerInput;
            _scoreElements = _layoutParent.GetComponentsInChildren<ScoreElement>(true).ToList();
            _restartDelay = gameRestartDelay;
            InitDisconnectPanel();
            _playerInput.OnEscapePressed += SwitchDisconnectButton;
        }

        private void InitDisconnectPanel()
        {
            _disconnectButton = _disconnectPanel.GetComponentInChildren<Button>(true);
            _disconnectPanel.gameObject.SetActive(false);
            _disconnectButton.onClick.AddListener(Disconnect);
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

        private void SwitchDisconnectButton()
        {
            _disconnectPanel.gameObject.SetActive(!_disconnectPanel.activeInHierarchy);
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
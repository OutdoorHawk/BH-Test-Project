using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.Player.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private Transform _layoutParent;
        [SerializeField] private GameObject _endGamePlate;
        [SerializeField] private Text _winPlayerText;
        [SerializeField] private Text _countDownText;
        [SerializeField] private Button _disconnectButton;

        private List<ScoreElement> _scoreElements = new();

        private float _restartDelay;

        public void Init(float gameRestartDelay)
        {
            _scoreElements = _layoutParent.GetComponentsInChildren<ScoreElement>(true).ToList();
            _disconnectButton.onClick.AddListener(Disconnect);
            _restartDelay = gameRestartDelay;
        }

        public void AddPlayerToScoreTable(PlayerConnectedMessage msg)
        {
            for (int i = 0; i < _scoreElements.Count; i++)
            {
                if (!_scoreElements[i].Active)
                {
                    _scoreElements[i].SetNetId((int)msg.NetId);
                    _scoreElements[i].SetName(msg.PlayerName);
                    _scoreElements[i].ActivateElement();
                    break;
                }
            }
        }

        public void UpdatePlayerScore(uint netID, int newScore)
        {
            for (var i = 0; i < _scoreElements.Count; i++)
            {
                var element = _scoreElements[i];
                if (element.NetId == netID)
                    element.SetScore(newScore);
            }
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

        private void Disconnect()
        {
            NetworkClient.Disconnect();
        }

        private void OnDestroy()
        {
            _disconnectButton.onClick.RemoveListener(Disconnect);
        }
    }
}
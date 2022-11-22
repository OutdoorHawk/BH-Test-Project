using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.Player.UI
{
    public class PlayerGameUI : MonoBehaviour
    {
        [SerializeField] private Transform _layoutParent;
        [SerializeField] private GameObject _endGamePlate;
        [SerializeField] private Text _winPlayerText;
        [SerializeField] private Text _countDownText;

        private List<ScoreElement> _scoreElements = new();

        private const int RESTART_DELAY = 3;

        private void Awake()
        {
            _scoreElements = _layoutParent.GetComponentsInChildren<ScoreElement>(true).ToList();
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
        public void AddPlayerToScoreTable(uint netID, string playerName)
        {
            for (int i = 0; i < _scoreElements.Count; i++)
            {
                if (!_scoreElements[i].Active)
                {
                    _scoreElements[i].SetNetId((int)netID);
                    _scoreElements[i].SetName(playerName);
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

        public void ResetPlayerScores()
        {
            for (var i = 0; i < _scoreElements.Count; i++)
            {
                if (_scoreElements[i].Active)
                    _scoreElements[i].SetScore(0);
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
            float countdown = RESTART_DELAY;
            do
            {
                _countDownText.text = countdown.ToString("0");
                countdown -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            } while (countdown > 0);
        }
    }
}
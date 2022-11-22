using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player.UI
{
    public class PlayerGameUI : MonoBehaviour
    {
        [SerializeField] private Transform _layoutParent;

        private List<ScoreElement> _scoreElements = new();

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

        public void UpdatePlayerScore(uint netID)
        {
            for (var i = 0; i < _scoreElements.Count; i++)
            {
                var element = _scoreElements[i];
                if (element.NetId == netID)
                    element.IncreaseScore();
            }
        }
    }
}
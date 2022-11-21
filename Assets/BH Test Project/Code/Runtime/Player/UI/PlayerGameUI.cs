using System;
using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.UI;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player.UI
{
    public class PlayerGameUI : NetworkBehaviour
    {
        [SerializeField] private Transform _layoutParent;

        private List<ScoreElement> _scoreElements = new();

        private void Awake()
        {
            _scoreElements = _layoutParent.GetComponentsInChildren<ScoreElement>(true).ToList();
        }

        public void AddPlayerToScoreTable(PlayerConnectedMessage msg)
        {
            _scoreElements[msg.Id].SetName(msg.PlayerName);
            _scoreElements[msg.Id].SetScore(0);
            _scoreElements[msg.Id].gameObject.SetActive(true);
        }
    }
}
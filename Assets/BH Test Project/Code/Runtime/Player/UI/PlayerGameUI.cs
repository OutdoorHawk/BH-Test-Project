using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using Mirror;
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
            _scoreElements[msg.Id].SetName(msg.PlayerName);
            _scoreElements[msg.Id].SetScore(0);
           _scoreElements[msg.Id].SetNetId((int)msg.NetId);
            _scoreElements[msg.Id].gameObject.SetActive(true);
        }
        
        public void UpdatePlayerScore(int newScore, uint netID)
        {
            for (var i = 0; i < _scoreElements.Count; i++)
            {
                var element = _scoreElements[i];
                if (element.GetNetId() == netID) 
                    element.SetScore(newScore);
            }
        }
    }
}
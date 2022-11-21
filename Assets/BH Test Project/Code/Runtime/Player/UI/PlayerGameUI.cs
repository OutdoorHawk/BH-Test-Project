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
            _scoreElements[msg.Id].SetName(msg.PlayerName);
            _scoreElements[msg.Id].SetScore(0);
            _scoreElements[msg.Id].SetNetId((int)msg.NetId);
            _scoreElements[msg.Id].gameObject.SetActive(true);
        }

        public void UpdatePlayerScore(uint netID)
        {
            _scoreElements[0].SetScore(3941);
            _scoreElements[1].SetScore(234);
            _scoreElements[2].SetScore(300);
            Debug.Log("score"+ netID);
            for (var i = 0; i < _scoreElements.Count; i++)
            {
                var element = _scoreElements[i];
                if (element.GetNetId() == netID)
                    element.SetScore(15);
            }
        }
    }
}
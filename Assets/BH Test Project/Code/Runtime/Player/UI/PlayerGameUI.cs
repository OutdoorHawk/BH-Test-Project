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
        

        public void Awake()
        {
            _scoreElements = _layoutParent.GetComponentsInChildren<ScoreElement>(true).ToList();
        }

        public void AddPlayerToScoreTable(PlayerOnServer newItem, int itemIndex)
        {
            _scoreElements[itemIndex].SetName(newItem.Name);
            _scoreElements[itemIndex].SetScore(newItem.Score);
            _scoreElements[itemIndex].gameObject.SetActive(true);
        }

        [ClientRpc]
        public void RpcAddPlayerToScoreTable(string playerName, NetworkIdentity identity)
        {
           
        }

      
    }
}
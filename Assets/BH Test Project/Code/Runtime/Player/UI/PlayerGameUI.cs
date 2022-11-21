using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.UI
{
    public class PlayerGameUI : NetworkBehaviour
    {
        [SerializeField] private Transform _layoutParent;

        private List<ScoreElement> _scoreElements = new();

        private readonly SyncList<ScoreEntity> _entities = new();

        public void Init()
        {
            _scoreElements = _layoutParent.GetComponentsInChildren<ScoreElement>(true).ToList();
            if (!isServer)
                return;
            for (int i = 0; i < _scoreElements.Count; i++)
                _entities.Add(new ScoreEntity());
        }

        [ClientRpc]
        public void RpcAddPlayerToScoreTable(string playerName, NetworkIdentity identity)
        {
            TakeScoreElement(playerName, identity);
        }
        
        private void TakeScoreElement(string playerName, NetworkIdentity identity)
        {
            for (var i = 0; i < _entities.Count; i++)
            {
                if (_entities[i].Identity == null)
                {
                    _entities[i].Identity = identity;
                    _entities[i].PlayerName = playerName;
                    _scoreElements[i].SetName(playerName);
                    _scoreElements[i].gameObject.SetActive(true);
                    return;
                }
            }
        }

        public void RemovePlayerFromScoreTable(PlayerOnServer playerOnServer)
        {
        }
    }
}
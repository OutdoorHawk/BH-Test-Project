using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Network;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.UI
{
    public class PlayerGameUI : NetworkBehaviour
    {
        [SerializeField] private Transform _layoutParent;
        [SerializeField] private ScoreElement _scoreElementPrefab;

        private readonly List<ScoreElement> _scoreElements = new();

        public void Init()
        {
            //NetworkClient.RegisterPrefab(_scoreElementPrefab.gameObject);
        }

        [ClientRpc]
        public void RpcAddPlayerToScoreTable(string playerName, uint netID)
        {
            ScoreElement element = Instantiate(_scoreElementPrefab, _layoutParent);
            element.Init(playerName, netID);
            _scoreElements.Add(element);
        }

        public void RemovePlayerFromScoreTable(PlayerOnServer playerOnServer)
        {
            foreach (var element in _scoreElements.Where(element => element.NetId == playerOnServer.NetID))
            {
                _scoreElements.Remove(element);
                Destroy(element);
            }
        }
    }
}
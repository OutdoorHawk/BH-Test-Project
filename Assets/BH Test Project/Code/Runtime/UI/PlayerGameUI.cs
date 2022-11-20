using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Network;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.UI
{
    public class PlayerGameUI : MonoBehaviour
    {
        [SerializeField] private Transform _layoutParent;
        [SerializeField] private ScoreElement _scoreElementPrefab;

        private readonly List<ScoreElement> _scoreElements = new();

        public void AddPlayerToScoreTable(PlayerOnServer playerOnServer)
        {
            ScoreElement element = Instantiate(_scoreElementPrefab, _layoutParent);
            element.Init(playerOnServer.Name, playerOnServer.NetID);
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
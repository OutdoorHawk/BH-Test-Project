using System;
using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.Lobby
{
    public class PlayerSlotView : MonoBehaviour
    {
        [SerializeField] private Text _playerName;
        [SerializeField] private Toggle _isReadyToggle;
        [SerializeField] private GameObject _slot;
        private string _defaultText;

        private void Awake()
        {
            _defaultText = _playerName.text;
        }

        public void ConnectPlayer(string playerName)
        {
            _playerName.text = playerName;
            _slot.gameObject.SetActive(true);
        }

        public void ClearPlayer()
        {
            _playerName.text = _defaultText;
            _slot.gameObject.SetActive(false);
        }
        
        public void SetReady(bool isReady)
        {
            _isReadyToggle.isOn = isReady;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.Lobby
{
    public class LobbyMenuWindow : MonoBehaviour
    {
        [SerializeField] private Button _readyButton;
        [SerializeField] private Button _leaveButton;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Transform _playerSlotsParent;
        [SerializeField] private int _minPlayersToStartGame = 2;

        private RoomPlayer[] _playerSlots;
        public Transform PlayerSlotsParent => _playerSlotsParent;

        public void InitLobby(bool IsHost)
        {
            if (IsHost)
                _startGameButton.gameObject.SetActive(true);
        }

        public void AddNewPlayerToLobby(Transform roomPlayer)
        {
            roomPlayer.SetParent(_playerSlotsParent);
            roomPlayer.SetSiblingIndex(0);
        }
        
        private bool IsEveryoneReady()
        {
            if (_playerSlots.Length < _minPlayersToStartGame)
                return false;
            return _playerSlots.All(player => player.readyToBegin);
        }

        private void DisconnectLobby()
        {
            /*if (isServer)
                NetworkServer.DisconnectAll();
            if (isClient)
                NetworkClient.Disconnect();*/
            // gameObject.SetActive(false);

            Debug.Log("disconnect");
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Data;
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

        private int _minPlayersToStartGame = 1;

        private List<RoomPlayer> _roomPlayers;
        public Transform PlayerSlotsParent => _playerSlotsParent;

        public void InitLobby(bool IsHost, int minPlayers)
        {
            if (IsHost)
            {
                _startGameButton.gameObject.SetActive(true);
                _startGameButton.onClick.AddListener(StartGame);
            }

            _leaveButton.onClick.AddListener(DisconnectLobby);
            _roomPlayers = new List<RoomPlayer>();
            _minPlayersToStartGame = minPlayers;
        }

        private void StartGame()
        {
            CleanUp();
            NetworkManager.singleton.ServerChangeScene(Constants.GAME_SCENE_NAME);
        }

        public void AddNewPlayerToLobby(Transform roomPlayer)
        {
            roomPlayer.SetParent(_playerSlotsParent);
            roomPlayer.SetSiblingIndex(0);
            roomPlayer.TryGetComponent(out RoomPlayer player);
            if (!_roomPlayers.Contains(player))
            {
                _roomPlayers.Add(player);
                player.OnRoomPlayerStateChanged += CheckGameCanStart;
            }
        }

        private void CheckGameCanStart()
        {
            _startGameButton.interactable = IsEveryoneReady();
        }

        private bool IsEveryoneReady()
        {
            if (_roomPlayers.Count < _minPlayersToStartGame)
                return false;
            return _roomPlayers.All(player => player.IsReady);
        }

        private void DisconnectLobby()
        {
            NetworkClient.Disconnect();
        }

        private void CleanUp()
        {
            _leaveButton.onClick.RemoveListener(DisconnectLobby);
            foreach (var pl in _roomPlayers)
            {
                pl.OnRoomPlayerStateChanged -= CheckGameCanStart;
                pl.transform.SetParent(null);
            }
        }
    }
}
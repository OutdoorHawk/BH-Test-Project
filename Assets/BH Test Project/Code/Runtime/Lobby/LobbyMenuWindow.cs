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
        [SerializeField] private Button _leaveButton;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Transform _playerSlotsParent;

        private int _minPlayersToStartGame;
        private List<RoomPlayer> _roomPlayers;

        public void InitLobby(bool IsHost, int minPlayers)
        {
            if (IsHost) 
                EnableStartGameButton();

            _leaveButton.onClick.AddListener(LeaveLobbyButtonPressed);
            _roomPlayers = new List<RoomPlayer>();
            _minPlayersToStartGame = minPlayers;
        }

        private void EnableStartGameButton()
        {
            _startGameButton.gameObject.SetActive(true);
            _startGameButton.onClick.AddListener(StartGame);
        }

        public void AddNewPlayerToLobby(Transform roomPlayer)
        {
            roomPlayer.SetParent(_playerSlotsParent);
            roomPlayer.localScale = Vector3.one;;
            roomPlayer.SetSiblingIndex(0);
            roomPlayer.TryGetComponent(out RoomPlayer player);
            if (!_roomPlayers.Contains(player))
            {
                _roomPlayers.Add(player);
                player.OnRoomPlayerStateChanged += CheckStartButtonAvailable;
            }
        }

        private void StartGame()
        {
            CleanUp();
            NetworkManager.singleton.ServerChangeScene(Constants.GAME_SCENE_NAME);
        }

        private void CheckStartButtonAvailable()
        {
            _startGameButton.interactable = IsEveryoneReady();
        }

        private bool IsEveryoneReady()
        {
            if (_roomPlayers.Count < _minPlayersToStartGame)
                return false;
            return _roomPlayers.All(player => player.IsReady);
        }

        private void LeaveLobbyButtonPressed()
        {
            NetworkClient.Disconnect();
            UpdatePlayersList();
        }

        private void UpdatePlayersList()
        {
            for (int i = 0; i < _roomPlayers.Count; i++)
            {
                if (_roomPlayers[i] == null)
                    _roomPlayers.RemoveAt(i);
            }
        }

        private void CleanUp()
        {
            _leaveButton.onClick.RemoveListener(LeaveLobbyButtonPressed);
            foreach (var pl in _roomPlayers)
            {
                pl.OnRoomPlayerStateChanged -= CheckStartButtonAvailable;
                pl.transform.SetParent(null);
            }
        }
    }
}
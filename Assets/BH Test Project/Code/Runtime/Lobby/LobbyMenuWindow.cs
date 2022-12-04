using System;
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
        public event Action OnLeaveButtonPressed;

        [SerializeField] private Button _leaveButton;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Transform _playerSlotsParent;

        private List<RoomPlayer> _roomPlayers;
        private int _minPlayersToStartGame;
        private bool _isServer;

        public void InitLobby(bool isServer, int minPlayers)
        {
            _isServer = isServer;
            _roomPlayers = new List<RoomPlayer>();
            _minPlayersToStartGame = minPlayers;
            _leaveButton.onClick.AddListener(LeaveLobbyButtonPressed);

            if (_isServer)
                EnableStartGameButton();
        }

        private void EnableStartGameButton()
        {
            _startGameButton.gameObject.SetActive(true);
            _startGameButton.onClick.AddListener(StartGame);
        }

        private void LeaveLobbyButtonPressed()
        {
            UpdatePlayersList();
            OnLeaveButtonPressed?.Invoke();
        }

        private void StartGame()
        {
            _startGameButton.onClick.RemoveListener(StartGame);
            NetworkManager.singleton.ServerChangeScene(Constants.GAME_SCENE_NAME);
        }

        public void UpdatePlayersInLobby(List<NetworkRoomPlayer> roomSlots)
        {
            for (var i = 0; i < roomSlots.Count; i++)
            {
                RoomPlayer player = SetPlayerToSlotPosition(roomSlots, i);
                if (!_roomPlayers.Contains(player))
                    _roomPlayers.Add(player);
            }

            CheckStartButtonAvailable();
        }

        private RoomPlayer SetPlayerToSlotPosition(List<NetworkRoomPlayer> roomSlots, int i)
        {
            Transform playerTransform = roomSlots[i].transform;
            playerTransform.transform.SetParent(_playerSlotsParent);
            playerTransform.transform.localScale = Vector3.one;
            playerTransform.transform.SetSiblingIndex(i);
            playerTransform.TryGetComponent(out RoomPlayer player);
            return player;
        }

        public void CheckStartButtonAvailable()
        {
            _startGameButton.interactable = IsEveryoneReady();
        }

        private bool IsEveryoneReady()
        {
            if (_roomPlayers.Count < _minPlayersToStartGame)
                return false;
            return _roomPlayers.All(player => player.IsReady);
        }

        private void UpdatePlayersList()
        {
            for (int i = 0; i < _roomPlayers.Count; i++)
            {
                if (_roomPlayers[i] == null)
                    _roomPlayers.RemoveAt(i);
            }
        }

        public void CleanUp()
        {
            _leaveButton.onClick.RemoveListener(LeaveLobbyButtonPressed);
            foreach (var pl in _roomPlayers)
            {
                if (pl == null)
                    return;
                pl.transform.SetParent(null);
                DontDestroyOnLoad(pl);
            }
        }
    }
}
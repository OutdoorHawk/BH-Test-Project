using System;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.Lobby
{
    public class RoomPlayer : NetworkRoomPlayer
    {
        public event Action OnRoomPlayerStateChanged;

        [SerializeField] private Text _playerNameText;
        [SerializeField] private Toggle _isReadyToggle;
        [SerializeField] private GameObject _slot;

        [SyncVar(hook = nameof(PlayerNameChanged))] private string _playerName;
        [SyncVar(hook = nameof(ReadyToggleChanged))] private bool _isReady;
        public bool IsReady => _isReady;

        private new void Start()
        {
            base.Start();
            if (isOwned)
            {
                _isReadyToggle.interactable = true;
                CmdRefreshLobbyUI();
                CmdPlayerNameSet(PlayerPrefs.GetString(Constants.PLAYER_NAME));
            }

            _isReadyToggle.onValueChanged.AddListener(CmdChangePlayerReadyState);
        }

        [Command(requiresAuthority = false)]
        private void CmdRefreshLobbyUI()
        {
            RoomPlayerAddedMessage msg = new RoomPlayerAddedMessage();
            NetworkServer.SendToAll(msg);
        }

        [Command(requiresAuthority = false)]
        private void CmdPlayerNameSet(string playerName)
        {
            _playerName = playerName;
        } 
        
        [Command(requiresAuthority = false)]
        private void CmdChangePlayerReadyState(bool value)
        {
            _isReady = value;
        }
        
        public  void ReadyToggleChanged(bool oldReadyState, bool newReadyState)
        {
            _isReadyToggle.isOn = newReadyState;
            OnRoomPlayerStateChanged?.Invoke();
        }

        private void PlayerNameChanged(string oldValue, string newValue)
        {
            _playerNameText.text = newValue;
        }

        public void ClearPlayer()
        {
            _slot.gameObject.SetActive(false);
        }

        public void SetReady(bool isReady)
        {
            _isReadyToggle.isOn = isReady;
        }

        private void OnDestroy()
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }
}
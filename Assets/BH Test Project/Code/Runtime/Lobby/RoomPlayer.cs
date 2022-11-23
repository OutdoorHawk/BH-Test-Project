using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.Lobby
{
    public class RoomPlayer : NetworkRoomPlayer
    {
        [SerializeField] private Text _playerNameText;
        [SerializeField] private Toggle _isReadyToggle;
        [SerializeField] private GameObject _slot;

        [SyncVar(hook = nameof(UpdateToggle))] private bool _isReady;
        [SyncVar(hook = nameof(UpdatePlayerName))] private string _playerName;

        private new void Start()
        {
            base.Start();
            if (isOwned)
                CmdRefreshLobbyUI();
            if (isOwned)
                CmdPlayerNameSet(PlayerPrefs.GetString(Constants.PLAYER_NAME));

            _isReadyToggle.onValueChanged.AddListener(CmdToggleChanged);
        }

        [Command(requiresAuthority = false)]
        private void CmdRefreshLobbyUI()
        {
            RoomPlayerAddedMessage msg = new RoomPlayerAddedMessage
            {
                NetId = netId
            };
            NetworkServer.SendToAll(msg);
        }

        [Command(requiresAuthority = false)]
        private void CmdPlayerNameSet(string playerName)
        {
            _playerName = playerName;
        }

        [Command(requiresAuthority = false)]
        private void CmdToggleChanged(bool value)
        {
            _isReady = value;
        }

        private void UpdateToggle(bool oldValue, bool newValue)
        {
            _isReadyToggle.isOn = newValue;
        }

        private void UpdatePlayerName(string oldValue, string newValue)
        {
            _playerNameText.text = newValue;
        }

        public void SetPlayerName(string msgPlayerName)
        {
            //if (isOwned)
                // _playerName.text = msgPlayerName;
        }


        public void ClearPlayer()
        {
            _slot.gameObject.SetActive(false);
        }

        public void SetReady(bool isReady)
        {
            _isReadyToggle.isOn = isReady;
        }
    }
}
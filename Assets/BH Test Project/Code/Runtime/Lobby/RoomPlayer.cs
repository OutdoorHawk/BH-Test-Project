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
        
        [SyncVar(hook = nameof(PlayerNameChanged))] private string _playerName;

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
            RoomPlayerAddedMessage msg = new RoomPlayerAddedMessage();
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
            readyToBegin = value;
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            base.ReadyStateChanged(oldReadyState, newReadyState);
            _isReadyToggle.isOn = newReadyState;
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
    }
}
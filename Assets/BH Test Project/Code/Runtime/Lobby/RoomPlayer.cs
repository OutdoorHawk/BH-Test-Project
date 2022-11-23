using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.Lobby
{
    public class RoomPlayer : NetworkRoomPlayer
    {
        [SerializeField] private Text _playerName;
        [SerializeField] private Toggle _isReadyToggle;
        [SerializeField] private GameObject _slot;

        [SyncVar(hook = nameof(UpdateToggle))] private bool _isReady;

        private new void Start()
        {
            base.Start();
            if (isOwned)
                CmdRefreshLobbyUI(PlayerPrefs.GetString(Constants.PLAYER_NAME));

            _isReadyToggle.onValueChanged.AddListener(OnToggleChanged);
        }
        

        [Command(requiresAuthority = false)]
        private void CmdRefreshLobbyUI(string playerName)
        {
            RoomPlayerAddedMessage msg = new RoomPlayerAddedMessage
            {
                NetId = netId,
                PlayerName = playerName
            };
            NetworkServer.SendToAll(msg);
        }

        [Command(requiresAuthority = false)]
        private void OnToggleChanged(bool value)
        {
            _isReady = value;
        }

        private void UpdateToggle(bool oldValue, bool newValue)
        {
            _isReadyToggle.isOn = newValue;
        }
        
        public void SetPlayerName(string msgPlayerName)
        {
            if (isOwned) 
                _playerName.text = msgPlayerName;
        }

        public void ConnectPlayer(string playerName)
        {
            _playerName.text = playerName;
            _slot.gameObject.SetActive(true);
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
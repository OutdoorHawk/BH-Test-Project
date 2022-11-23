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
        private string _defaultText;

        private new void Start()
        {
            base.Start();
            Debug.Log("send");
            CmdRefreshLobbyUI();
        }

        [Command(requiresAuthority = false)]
        private void CmdRefreshLobbyUI()
        {
            NetworkServer.SendToAll(new RoomPlayerAddedMessage());
        }

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
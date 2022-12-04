using System;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Infrastructure.Network.Lobby;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.UI;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Runtime.Player.Systems;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.Lobby
{
    [RequireComponent(typeof(PlayerNameComponent))]
    public class RoomPlayer : NetworkRoomPlayer
    {
        public event Action OnRoomPlayerStateChanged;

        [SerializeField] private Text _playerNameText;
        [SerializeField] private Toggle _isReadyToggle;

        [SyncVar(hook = nameof(ReadyToggleChanged))] private bool _isReady;

        private IUIFactory _uiFactory;
        private PlayerNameComponent _playerNameComponent;
        private LobbyMenuWindow _lobbyMenuWindow;
        private IGameStateMachine _gameStateMachine;
        private ServerInfoComponent _serverInfo;
        private INetworkManagerService _networkService;

        public bool IsReady => _isReady;

        /*
    At the moment, I have not found any way to transfer the dependency from the outside. 
    Since client or target rpc does not allow to transfer complex data. Through custom writer, I also can't serialize complex services. 
    There are two options left, to get the service from static or 
    reinitialize all players each time, when a new player is connected. (bool check will be required).
    https://mirror-networking.gitbook.io/docs/guides/data-types
      */

        [ClientRpc]
        public void RpcConstruct()
        {
            if (!isOwned)
                return;
            _gameStateMachine = DIContainer.Container.Resolve<IGameStateMachine>();
            _uiFactory = DIContainer.Container.Resolve<IUIFactory>();
            _networkService = DIContainer.Container.Resolve<INetworkManagerService>();
        }

        [ClientRpc]
        public void RpcInitializePlayer()
        {
            InitNameComponent();
            if (!isOwned)
                return;

            InitPlayer();
            CreateLobbyUI();
            _isReadyToggle.onValueChanged.AddListener(CmdChangePlayerReadyState);
        }

        private void InitNameComponent()
        {
            _playerNameComponent = GetComponent<PlayerNameComponent>();
            _playerNameComponent.OnNameChanged += OnPlayerNameChanged;
        }

        private void InitPlayer()
        {
            InitPlayerNameComponent();
            _isReadyToggle.interactable = true;
        }

        private void InitPlayerNameComponent()
        {
            CmdSetPlayerName(PlayerPrefs.GetString(Constants.PLAYER_NAME));
        }

        private void CreateLobbyUI()
        {
            _lobbyMenuWindow = _uiFactory.CreateLobbyMenuWindow();
            _lobbyMenuWindow.InitLobby(isServer, _networkService.MinPlayersToStart);
        }

        public void UpdatePlayerUI()
        {
            if (!isOwned)
                return;
            _lobbyMenuWindow.UpdatePlayersInLobby(_networkService.PlayersInRoom);
        }

        [Command(requiresAuthority = false)]
        private void CmdChangePlayerReadyState(bool value)
        {
            _isReady = value;
        }

        [Command(requiresAuthority = false)]
        private void CmdSetPlayerName(string playerName)
        {
            _playerNameComponent.SetPlayerName(playerName);
        }

        public void ReadyToggleChanged(bool oldReadyState, bool newReadyState)
        {
            _isReadyToggle.isOn = newReadyState;
            OnRoomPlayerStateChanged?.Invoke();
        }

        private void OnPlayerNameChanged(string newValue)
        {
            _playerNameText.text = newValue;
        }

        private void OnDestroy()
        {
            if (isOwned && _playerNameComponent != null)
                _playerNameComponent.OnNameChanged -= OnPlayerNameChanged;
          
        }
    }
}
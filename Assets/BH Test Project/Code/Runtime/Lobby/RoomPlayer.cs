using System;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Infrastructure.Network.Lobby;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.UI;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;
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

        private IUIFactory _uiFactory;
        private PlayerNameComponent _playerNameComponent;
        private LobbyMenuWindow _lobbyMenuWindow;
        private IGameStateMachine _gameStateMachine;
        private ServerInfoComponent _serverInfo;
        private INetworkManagerService _networkService;

        [field: SyncVar(hook = nameof(ReadyToggleChanged))] public bool IsReady { get; private set; }
        [field: SyncVar(hook = nameof(PlayerNameChanged))] public string PlayerName { get; private set; }

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
            _playerNameComponent = GetComponent<PlayerNameComponent>();
            _gameStateMachine = DIContainer.Container.Resolve<IGameStateMachine>();
            _uiFactory = DIContainer.Container.Resolve<IUIFactory>();
            _networkService = DIContainer.Container.Resolve<INetworkManagerService>();
        }

        [ClientRpc]
        public void RpcInitializePlayer()
        {
            if (!isOwned)
                return;

            InitPlayer();
            CreateLobbyUI();
            _isReadyToggle.onValueChanged.AddListener(CmdChangePlayerReadyState);
        }

        private void InitPlayer()
        {
            CmdSetPlayerName(PlayerPrefs.GetString(Constants.PLAYER_NAME));
            _isReadyToggle.interactable = true;
        }

        [Command]
        private void CmdSetPlayerName(string playerName)
        {
            PlayerName = playerName;
        }

        private void CreateLobbyUI()
        {
            _lobbyMenuWindow = _uiFactory.CreateLobbyMenuWindow();
            _lobbyMenuWindow.InitLobby(isServer, _networkService.MinPlayersToStart);
            _lobbyMenuWindow.OnLeaveButtonPressed += DisconnectFromLobby;
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
            IsReady = value;
        }

        public void ReadyToggleChanged(bool oldReadyState, bool newReadyState)
        {
            _isReadyToggle.isOn = newReadyState;
            OnRoomPlayerStateChanged?.Invoke();
        }

        private void PlayerNameChanged(string _, string newValue)
        {
            _playerNameText.text = newValue;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            DisconnectFromLobby();
        }

        private void DisconnectFromLobby()
        {
            if (isServer && isOwned)
                _networkService.StopServer();
            _lobbyMenuWindow.OnLeaveButtonPressed -= DisconnectFromLobby;
            _gameStateMachine.Enter<MainMenuState>();
            NetworkClient.Disconnect();
        }

    }
}
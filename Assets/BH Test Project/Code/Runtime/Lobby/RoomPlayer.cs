using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.DI;
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
        [SerializeField] private Text _playerNameText;
        [SerializeField] private Toggle _isReadyToggle;

        private IUIFactory _uiFactory;
        private LobbyMenuWindow _lobbyMenuWindow;
        private IGameStateMachine _gameStateMachine;
        private IGameNetworkService _gameNetworkService;

        [field: SyncVar(hook = nameof(HandleNameChanged))] public string PlayerName { get; private set; }
        [field: SyncVar(hook = nameof(HandleToggleChanged))] public bool IsReady { get; private set; }

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
            _gameStateMachine = DIContainer.Container.Resolve<IGameStateMachine>();
            _uiFactory = DIContainer.Container.Resolve<IUIFactory>();
            _gameNetworkService = DIContainer.Container.Resolve<IGameNetworkService>();
        }

        [ClientRpc]
        public void RpcInitializePlayer()
        {
            if (!isOwned)
                return;

            InitPlayer();
            CreateLobbyUI();
            Subscribe();
            CmdAddPlayerProfile(PlayerPrefs.GetString(Constants.PLAYER_NAME));
        }

        [Command]
        private void CmdAddPlayerProfile( string playerName)
        {
            _gameNetworkService.AddPlayerProfile( playerName);
        }
        
        public void RpcUpdatePlayerUI()
        {
            if (!isOwned)
                return;

            _lobbyMenuWindow.UpdatePlayersInLobby(_gameNetworkService.PlayersInRoom);
        }

        private void InitPlayer()
        {
            CmdSetPlayerName(PlayerPrefs.GetString(Constants.PLAYER_NAME));
            _isReadyToggle.interactable = true;
        }

        private void CreateLobbyUI()
        {
            _lobbyMenuWindow = _uiFactory.CreateLobbyMenuWindow();
            _lobbyMenuWindow.InitLobby(isServer, _gameNetworkService.MinPlayersToStart);
        }

        private void Subscribe()
        {
            _isReadyToggle.onValueChanged.AddListener(CmdChangePlayerReadyState);
            _lobbyMenuWindow.OnLeaveButtonPressed += DisconnectFromLobby;
            _lobbyMenuWindow.OnStartButtonPressed += StartGame;
        }

        private void StartGame()
        {
            _lobbyMenuWindow.CleanUp();
            _gameNetworkService.LoadGameLevel();
        }

        private void Unsubscribe()
        {
            _isReadyToggle.onValueChanged.RemoveListener(CmdChangePlayerReadyState);
            _lobbyMenuWindow.OnLeaveButtonPressed -= DisconnectFromLobby;
            _lobbyMenuWindow.OnStartButtonPressed -= StartGame;
        }

        [Command]
        private void CmdSetPlayerName(string playerName)
        {
            PlayerName = playerName;
        }

        [Command]
        private void CmdChangePlayerReadyState(bool value)
        {
            IsReady = value;

            foreach (var player in _gameNetworkService.PlayersInRoom)
            {
                if (player.isServer && player is RoomPlayer roomPlayer)
                    roomPlayer.TargetCheckStartButton();
            }
        }

        [TargetRpc]
        private void TargetCheckStartButton()
        {
            if (!isOwned)
                return;
            _lobbyMenuWindow.CheckStartButtonAvailable();
        }

        private void HandleNameChanged(string _, string newValue) =>
            _playerNameText.text = newValue;

        public void HandleToggleChanged(bool oldReadyState, bool newReadyState) =>
            _isReadyToggle.isOn = newReadyState;

        public override void OnStopServer()
        {
            base.OnStopServer();
            DisconnectFromLobby();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            DisconnectFromLobby();
        }

        private void DisconnectFromLobby()
        {
            if (!isOwned)
                return;
            if (isServer)
                _gameNetworkService.StopServer();
            Unsubscribe();
            _gameStateMachine.Enter<MainMenuState>();
            NetworkClient.Disconnect();
        }
    }
}
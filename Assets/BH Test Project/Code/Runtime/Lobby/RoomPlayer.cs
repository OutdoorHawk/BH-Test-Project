using System;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network.Lobby;
using BH_Test_Project.Code.Infrastructure.Services;
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

        [SyncVar(hook = nameof(ReadyToggleChanged))] private bool _isReady;

        private IUIFactory _uiFactory;
        private PlayerNameComponent _playerNameComponent;
        private LobbyMenuWindow _lobbyMenuWindow;
        private IGameStateMachine _gameStateMachine;
        private ServerInfoComponent _serverInfo;

        public bool Initialized { get; private set; }
        public bool IsReady => _isReady;

        public void Construct(IGameStateMachine gameStateMachine, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
        }

        public void Init()
        {
            InitNameComponent();
            if (!isOwned)
                return;
            Debug.Log("init");
            InitPlayer();
            CreateLobbyUI();
            _isReadyToggle.onValueChanged.AddListener(CmdChangePlayerReadyState);
            Initialized = true;
        }

        public void UpdateUI()
        {
            if (!isOwned) 
                return;
            UpdateLobbyUI();
        }

        private new void Start()
        {
            base.Start();
            Debug.Log("start");
           
            /*InitNameComponent();
            if (!isOwned)
                return;
            InitPlayer();
            CreateLobbyUI();
            UpdateLobbyUI();
            _isReadyToggle.onValueChanged.AddListener(CmdChangePlayerReadyState);*/
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
            _gameStateMachine.Enter<LobbyState>();
        }

        private void InitPlayerNameComponent()
        {
            CmdSetPlayerName(PlayerPrefs.GetString(Constants.PLAYER_NAME));
        }

        private void CreateLobbyUI()
        {
            _lobbyMenuWindow = _uiFactory.CreateLobbyMenuWindow();
            if (NetworkManager.singleton is NetworkRoomManager room)
                _lobbyMenuWindow.InitLobby(isServer, room.minPlayers);
        }

        private void UpdateLobbyUI()
        {
            if (NetworkManager.singleton is NetworkRoomManager room)
                _lobbyMenuWindow.UpdatePlayersInLobby(room.roomSlots);
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
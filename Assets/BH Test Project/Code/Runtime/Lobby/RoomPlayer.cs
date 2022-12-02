using System;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.DI;
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

        public bool IsReady => _isReady;
        
        public void Init(IGameStateMachine gameStateMachine, IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
        }

        private new void Start()
        {
            base.Start();
            InitNameComponent();
            if (isOwned)
                InitPlayer();

            InitLobbyUI();
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
            _gameStateMachine.Enter<LobbyState>();
        }

        private void InitPlayerNameComponent()
        {
            SetPlayerName(PlayerPrefs.GetString(Constants.PLAYER_NAME));
        }

        private void InitLobbyUI()
        {
            _lobbyMenuWindow = _uiFactory.CreateLobbyMenuWindow();
            if (NetworkManager.singleton is NetworkRoomManager room)
            {
                _lobbyMenuWindow.InitLobby(isServer, room.minPlayers);
                _lobbyMenuWindow.UpdatePlayersInLobby(room.roomSlots);
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdChangePlayerReadyState(bool value)
        {
            _isReady = value;
        }

        [Command(requiresAuthority = false)]
        private void SetPlayerName(string playerName)
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
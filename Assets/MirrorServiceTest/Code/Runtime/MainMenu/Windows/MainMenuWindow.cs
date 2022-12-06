using MirrorServiceTest.Code.Infrastructure.Services.Network;
using MirrorServiceTest.Code.Infrastructure.StateMachine;
using MirrorServiceTest.Code.Infrastructure.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using static MirrorServiceTest.Code.Infrastructure.Data.Constants;

namespace MirrorServiceTest.Code.Runtime.MainMenu.Windows
{
    public class MainMenuWindow : MonoBehaviour
    {
        [SerializeField] private InputField _inputField;
        [SerializeField] private Button _joinGameButton;
        [SerializeField] private Button _hostGameButton;
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private EnterIpView _enterIpWindow;

        private IGameNetworkService _networkService;
        private IGameStateMachine _gameStateMachine;

        public void Init(IGameNetworkService game, IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _networkService = game;
            Subscribe();
        }

        private void Subscribe()
        {
            _joinGameButton.onClick.AddListener(JoinGameClicked);
            _hostGameButton.onClick.AddListener(HostGameClicked);
            _exitGameButton.onClick.AddListener(ExitGame);
            _enterIpWindow.OnJoinGamePressed += JoinGameClicked;
        }

        private void HostGameClicked()
        {
            if (!_networkService.CreateLobbyAsHost())
                return;
            SavePlayerName();
            _gameStateMachine.Enter<LobbyState>();
        }

        private void JoinGameClicked(string networkAddress)
        {
            if (!_networkService.JoinLobbyAsClient(networkAddress))
                return;
            SavePlayerName();
            _gameStateMachine.Enter<LobbyState>();
        }

        private void SavePlayerName()
        {
            PlayerPrefs.SetString(PLAYER_NAME,
                _inputField.text != "" ? _inputField.text : $"Player {Random.Range(0, 99)}");
        }

        private void JoinGameClicked()
        {
            _enterIpWindow.OpenWindow();
        }

        private void ExitGame()
        {
            Application.Quit();
        }

        private void CleanUp()
        {
            _joinGameButton.onClick.RemoveListener(JoinGameClicked);
            _hostGameButton.onClick.RemoveListener(HostGameClicked);
            _exitGameButton.onClick.RemoveListener(ExitGame);
            _enterIpWindow.OnJoinGamePressed -= JoinGameClicked;
        }

        private void OnDestroy()
        {
            CleanUp();
        }
    }
}
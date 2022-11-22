using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Runtime.MainMenu.Network;
using UnityEngine;
using UnityEngine.UI;
using static BH_Test_Project.Code.Infrastructure.Data.Constants;

namespace BH_Test_Project.Code.Runtime.MainMenu.Windows
{
    public class MainMenuWindow : MonoBehaviour
    {
        [SerializeField] private InputField _inputField;
        [SerializeField] private Button _joinGameButton;
        [SerializeField] private Button _hostGameButton;
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private EnterIpView _enterIpWindow;
        
        private GameNetworkManager _gameManager;

        public void Init(GameNetworkManager gameManager)
        {
            _gameManager = gameManager;
            Subscribe();
        }

        private void Subscribe()
        {
            _joinGameButton.onClick.AddListener(JoinGameClicked);
            _hostGameButton.onClick.AddListener(HostGameClicked);
            _exitGameButton.onClick.AddListener(ExitGame);
            _enterIpWindow.OnJoinGamePressed += JoinGameClicked;
        }

        private void DisableMainMenu()
        {
            gameObject.SetActive(false);
        }

        private void HostGameClicked()
        {
            _gameManager.CreateLobbyAsHost();
            SavePlayerName();
        }

        private void JoinGameClicked(string networkAddress)
        {
            _gameManager.JoinLobbyAsClient(networkAddress);
            SavePlayerName();
        }

        private void SavePlayerName()
        {
            if (_inputField.text != "")
                PlayerPrefs.SetString(PLAYER_NAME, _inputField.text);
            else
                PlayerPrefs.SetString(PLAYER_NAME, $"Player {Random.Range(0, 9)}");
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
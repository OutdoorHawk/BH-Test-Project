using BH_Test_Project.Code.Runtime.MainMenu.Network;
using UnityEngine;
using UnityEngine.UI;

namespace BH_Test_Project.Code.Runtime.MainMenu.Windows
{
    public class MainMenuWindow : MonoBehaviour
    {
        [SerializeField] private MainMenuNetworkSystem _menuNetwork;
        [SerializeField] private InputField _inputField;
        [SerializeField] private Button _joinGameButton;
        [SerializeField] private Button _hostGameButton;
        [SerializeField] private EnterIpView _enterIpWindow;

        private const string PLAYER_NAME = "Player name";

        private void Awake()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _joinGameButton.onClick.AddListener(JoinGameClicked);
            _hostGameButton.onClick.AddListener(HostGameClicked);
            _enterIpWindow.OnJoinGamePressed += JoinGameClicked;
        }

        private void HostGameClicked()
        {
            _menuNetwork.StartGameAsHost();
            SavePlayerName();
        }

        private void JoinGameClicked(string networkAddress)
        {
            _menuNetwork.StartGameAsClient(networkAddress);
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

        private void CleanUp()
        {
            _joinGameButton.onClick.RemoveListener(JoinGameClicked);
            _hostGameButton.onClick.RemoveListener(HostGameClicked);
            _enterIpWindow.OnJoinGamePressed -= JoinGameClicked;
        }

        private void OnDestroy()
        {
            CleanUp();
        }
    }
}
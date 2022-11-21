using BH_Test_Project.Code.Infrastructure.Data;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BH_Test_Project.Code.Runtime.MainMenu.Network
{
    public class MainMenuNetworkSystem : NetworkManager
    {
        public override void OnStartServer()
        {
            base.OnStartServer();
            SceneManager.LoadScene((int)LevelID.Lobby);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            SceneManager.LoadScene((int)LevelID.Lobby);
        }

        public void StartGameAsHost()
        {
            StartHost();
        }

        public void StartGameAsClient(string address)
        {
            networkAddress = address;
            StartClient();
        }
    }
}
using System;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.MainMenu.Network
{
    public class MainMenuNetworkSystem : NetworkManager
    {
        public event Action OnServerStarted;
        public event Action OnClientConnected;
        
        [SerializeField] private LobbyMenuWindow _lobbyMenu;

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            OnClientConnected?.Invoke();
            Debug.Log("OnClientConnect");
        }

        private void OnConnectedToServer()
        {
            Debug.Log("OnConnectedToServer");
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Debug.Log("OnStartClient");
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            OnServerStarted?.Invoke();
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
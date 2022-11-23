using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BH_Test_Project.Code.Runtime.MainMenu.Network
{
    public class GameNetworkManager : NetworkRoomManager
    {
        private IGameStateMachine _gameStateMachine;
        private ISceneContextService _sceneContextService;
        private NetworkSpawnSystem _spawnSystem;
        private IUIFactory _uiFactory;
        private LobbyMenuWindow _lobbyMenuWindow;

        public void Init(IGameStateMachine gameStateMachine, ISceneContextService sceneContextService,
            IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _sceneContextService = sceneContextService;
            _gameStateMachine = gameStateMachine;
        }

        public void CreateLobbyAsHost()
        {
            Debug.Log(NetworkServer.active);
            if (!NetworkServer.active)
                StartHost();
        }

        public void JoinLobbyAsClient(string address)
        {
            networkAddress = address;
            if (!NetworkClient.active && !NetworkServer.active)
                StartClient();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            NetworkClient.RegisterHandler<GameRestartMessage>(OnGameRestarted);
            NetworkClient.RegisterHandler<RoomPlayerAddedMessage>(OnRoomPlayerAdded);
        }

        private void OnRoomPlayerAdded(RoomPlayerAddedMessage msg)
        {
            for (int i = 0; i < roomSlots.Count; i++) 
                _lobbyMenuWindow.AddNewPlayerToLobby(roomSlots[i].transform);
        }

        /*public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
        {
            NetworkRoomPlayer go = Instantiate(roomPlayerPrefab, _lobbyMenuWindow.PlayerSlotsParent);
            return go.gameObject;
        }*/

        public override void OnRoomStartClient()
        {
            foreach (var roomPlayer in roomSlots)
                roomPlayer.transform.SetParent(_lobbyMenuWindow.PlayerSlotsParent);
            base.OnRoomStartClient();
        }

        public override void OnRoomClientConnect()
        {
            base.OnRoomClientConnect();

            _gameStateMachine.Enter<LobbyState>();
            _lobbyMenuWindow = _uiFactory.CreateLobbyMenuWindow();
            if (NetworkServer.active)
                NetworkServer.Spawn(_lobbyMenuWindow.gameObject);
            _lobbyMenuWindow.InitLobby(NetworkClient.isHostClient, minPlayers);
        }

        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();
            if (SceneManager.GetActiveScene().name == Constants.GAME_SCENE_NAME)
                _gameStateMachine.Enter<GameLoopState>();
        }

        private void OnGameRestarted(GameRestartMessage obj)
        {
            ServerChangeScene(GameplayScene);
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            StopServer();
            _gameStateMachine.Enter<MainMenuState>();
        }
        
    }
}
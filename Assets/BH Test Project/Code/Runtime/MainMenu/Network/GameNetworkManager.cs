using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;
using UnityEngine.SceneManagement;

namespace BH_Test_Project.Code.Runtime.MainMenu.Network
{
    public class GameNetworkManager : NetworkRoomManager
    {
        private IGameStateMachine _gameStateMachine;
        private ISceneContextService _sceneContextService;

        public void Init(IGameStateMachine gameStateMachine, ISceneContextService sceneContextService)
        {
            _sceneContextService = sceneContextService;
            _gameStateMachine = gameStateMachine;
        }

        public void CreateLobbyAsHost()
        {
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
            LobbyMenuWindow lobbyMenuWindow = _sceneContextService.GetLobbyMenuWindow();
            for (int i = 0; i < roomSlots.Count; i++)
                lobbyMenuWindow.AddNewPlayerToLobby(roomSlots[i].transform);
        }

        public override void OnRoomClientConnect()
        {
            base.OnRoomClientConnect();
            _gameStateMachine.Enter<LobbyState, int>(minPlayers);
        }

        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();
            if (SceneManager.GetActiveScene().name == Constants.GAME_SCENE_NAME)
                _gameStateMachine.Enter<GameLoopState>();
        }

        private void OnGameRestarted(GameRestartMessage msg)
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
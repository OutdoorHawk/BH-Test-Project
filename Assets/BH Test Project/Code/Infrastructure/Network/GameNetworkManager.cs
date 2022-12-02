using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;
using BH_Test_Project.Code.Runtime.Player.Systems;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class GameNetworkManager : NetworkRoomManager, INetworkManagerService
    {
        private IGameStateMachine _gameStateMachine;
        public static Dictionary<int, string> PlayerNames { get; } = new();

        public void Init(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void HandleSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (SceneManager.GetActiveScene().name == Constants.GAME_SCENE_NAME)
                _gameStateMachine.Enter<GameLoopState>();
        }

        public void CreateLobbyAsHost()
        {
            if (NetworkServer.active)
                return;
            _gameStateMachine.Enter<LobbyState>();
            StartHost();
        }

        public void JoinLobbyAsClient(string address)
        {
            networkAddress = address;
            if (NetworkClient.active || NetworkServer.active)
                return;
            _gameStateMachine.Enter<LobbyState>();
            StartClient();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            NetworkClient.RegisterHandler<GameRestartMessage>(OnGameRestarted);
        }

        public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn,
            GameObject roomPlayer)
        {
            GameObject player =
                Instantiate(playerPrefab, GetStartPosition().position, GetStartPosition().rotation);
            TransferNamesToGameScene(roomPlayer, player);

            NetworkServer.ReplacePlayerForConnection(conn, player);
            return player;
        }

        private static void TransferNamesToGameScene(GameObject roomPlayer, GameObject player)
        {
            if (roomPlayer.TryGetComponent(out PlayerNameComponent nameSender) &&
                player.TryGetComponent(out PlayerNameComponent nameReceiver))
                nameReceiver.SetPlayerName(nameSender.GetPlayerName());
        }

        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();
            NetworkClient.connection.owned.RemoveWhere(NullIdentity);
        }

        private void OnGameRestarted(GameRestartMessage msg)
        {
            ServerChangeScene(GameplayScene);
        }

        private bool NullIdentity(NetworkIdentity identity) => identity == null;

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            PlayerNames.Clear();
            StopServer();
            _gameStateMachine.Enter<MainMenuState>();
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }
    }
}
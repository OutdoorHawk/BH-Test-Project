using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.PlayerFactory;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class GameNetworkManager : NetworkRoomManager, INetworkManagerService
    {
        private IGameStateMachine _gameStateMachine;
        private IPlayerFactory _playerFactory;

        public static Dictionary<int, string> PlayerNames { get; } = new();
        public RoomPlayer RoomPlayerPrefab => roomPlayerPrefab as RoomPlayer;

        public void Init(IGameStateMachine gameStateMachine, IPlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
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
            StartHost();
        }

        public void JoinLobbyAsClient(string address)
        {
            networkAddress = address;
            if (NetworkClient.active || NetworkServer.active)
                return;
            StartClient();
        }

        public RoomPlayer GetPlayerForConnection(NetworkConnectionToClient conn)
        {
            foreach (var connection in NetworkServer.connections.Values.Where(connection => connection == conn))
                if (connection.identity.TryGetComponent(out RoomPlayer player))
                    return player;
            return null;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            NetworkClient.RegisterHandler<GameRestartMessage>(OnGameRestarted);
        }

        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);
            _playerFactory.CreateRoomPlayer(conn, RoomPlayerPrefab);
            Debug.Log(roomSlots.Count);
            //_playerFactory.InitPlayer(conn.identity.GetComponent<RoomPlayer>());
        }
        

        public override void OnRoomClientEnter() // new client spawned & added to room slot
        {
            base.OnRoomClientEnter();
            Debug.Log(roomSlots.Count);
            _playerFactory.InitializePlayers(roomSlots);
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
            //_gameStateMachine.Enter<MainMenuState>();
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        /*public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn,
            GameObject roomPlayer)
        {
            GameObject player =
                Instantiate(playerPrefab, GetStartPosition().position, GetStartPosition().rotation);
            TransferNamesToGameScene(roomPlayer, player);

            NetworkServer.ReplacePlayerForConnection(conn, player);
            return player;
        }*/

        /*private static void TransferNamesToGameScene(GameObject roomPlayer, GameObject player)
        {
            if (roomPlayer.TryGetComponent(out PlayerNameComponent nameSender) &&
                player.TryGetComponent(out PlayerNameComponent nameReceiver))
                nameReceiver.SetPlayerName(nameSender.GetPlayerName());
        }*/
    }
}
using System;
using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.PlayerFactory;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class GameNetworkManager : NetworkRoomManager, INetworkManagerService
    {
        public event Action<NetworkConnectionToClient> OnServerReadyEvent;
        public event Action OnRoomClientEnterEvent;
        public event Action<string> OnRoomClientSceneChangedEvent;

        private IPlayerFactory _playerFactory;
        
        public static Dictionary<int, string> PlayerNames { get; } = new();
        public List<NetworkRoomPlayer> PlayersInRoom => roomSlots;
        public RoomPlayer RoomPlayerPrefab => roomPlayerPrefab as RoomPlayer;
        public GameObject GamePlayerPrefab => playerPrefab;
        public int MinPlayersToStart => minPlayers;

        public void Init(IPlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
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

        public void LoadGameLevel()
        {
            ServerChangeScene(Constants.GAME_SCENE_NAME);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            NetworkClient.RegisterHandler<GameRestartMessage>(OnGameRestarted);
        }

        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            OnServerReadyEvent?.Invoke(conn);
            base.OnServerReady(conn);
        }

        public override void OnRoomClientEnter() // new client spawned & added to room slot
        {
            base.OnRoomClientEnter();
            OnRoomClientEnterEvent?.Invoke();
        }

        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation,
            bool customHandling)
        {
            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
            OnRoomClientSceneChangedEvent?.Invoke(newSceneName);
        }

        public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn,
            GameObject roomPlayer)
        {
            return _playerFactory.CreateGamePlayer(conn, playerPrefab).gameObject;
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn,
            GameObject roomPlayer,
            GameObject gamePlayer)
        {
            return base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);
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
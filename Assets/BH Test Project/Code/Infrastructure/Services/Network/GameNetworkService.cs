using System;
using System.Collections;
using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Services.PlayerFactory;
using BH_Test_Project.Code.Runtime.Lobby;
using BH_Test_Project.Code.Runtime.Player;
using BH_Test_Project.Code.StaticData;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services.Network
{
    public class GameNetworkService : NetworkRoomManager, IGameNetworkService
    {
        public event Action<NetworkConnectionToClient> OnServerReadyEvent;
        public event Action OnRoomClientEnterEvent;
        public event Action<string> OnRoomClientSceneChangedEvent;

        private IPlayerFactory _playerFactory;
        private ServerScoreSystem _serverScoreSystem;
        private float _gameRestartDelay;

        public List<NetworkRoomPlayer> PlayersInRoom => roomSlots;
        public RoomPlayer RoomPlayerPrefab => roomPlayerPrefab as RoomPlayer;
        public GameObject GamePlayerPrefab => playerPrefab;
        public int MinPlayersToStart => minPlayers;

        public void Init(IPlayerFactory playerFactory, WorldStaticData worldStaticData)
        {
            _playerFactory = playerFactory;
            _gameRestartDelay = worldStaticData.GameRestartDelay;
            _serverScoreSystem = new ServerScoreSystem(worldStaticData.GameEndScore);
        }

        public bool CreateLobbyAsHost()
        {
            if (NetworkServer.active)
                return false;
            StartHost();
            return true;
        }

        public bool JoinLobbyAsClient(string address)
        {
            networkAddress = address;
            if (NetworkClient.active || NetworkServer.active)
                return false;
            StartClient();
            return true;
        }

        public void LoadGameLevel()
        {
            ServerChangeScene(Constants.GAME_SCENE_NAME);
        }

        [Server]
        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);
            OnServerReadyEvent?.Invoke(conn);
        }

        [Client]
        public override void OnRoomClientEnter()
        {
            base.OnRoomClientEnter();
            OnRoomClientEnterEvent?.Invoke();
        }

        [Client]
        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation,
            bool customHandling)
        {
            OnRoomClientSceneChangedEvent?.Invoke(newSceneName);
            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
        }

        [Server]
        public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn,
            GameObject roomPlayer)
        {
            return _playerFactory.CreateGamePlayer(conn, playerPrefab).gameObject;
        }

        [Client]
        public override void OnClientSceneChanged()
        {
            NetworkClient.connection.owned.RemoveWhere(NullIdentity);
            base.OnClientSceneChanged();
        }

        private bool NullIdentity(NetworkIdentity identity) => identity == null;

        [Server]
        public void AddPlayerProfile(string playerName, int connID) => 
            _serverScoreSystem.AddProfileToServer(playerName, connID);

        [Server]
        public void UpdatePlayersHUD() => 
            _serverScoreSystem.SendUpdateHUDRpc();

        [Server]
        public void SendHitToPlayer(int targetID, int senderID) => 
            _serverScoreSystem.SendHitPlayerRpc(targetID, senderID);

        [Server]
        public void SendHitSuccess(int senderID)
        {
            _serverScoreSystem.SendHitSuccessRpc(senderID);
            _serverScoreSystem.CheckEndScoreReached(OnGameEnd);
        }

        [Server]
        private void OnGameEnd(string winnerName)
        {
            _serverScoreSystem.SendEndGameRpc(winnerName);
            StartCoroutine(RestartGameRoutine());
        }

        [Server]
        private IEnumerator RestartGameRoutine()
        {
            yield return new WaitForSeconds(_gameRestartDelay);
            _serverScoreSystem.ResetPlayersScore();
            ReplaceGamePlayersToRoomPlayers();
            LoadGameLevel();
        }

        [Server]
        private void ReplaceGamePlayersToRoomPlayers()
        {
            int i = 0;
            foreach (var conn in NetworkServer.connections.Values)
            {
                NetworkServer.DestroyPlayerForConnection(conn);
                NetworkServer.AddPlayerForConnection(conn, roomSlots[i].gameObject);
                i++;
            }
        }

        [Server]
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            SendDisconnectRpcToPlayer(conn);
            _serverScoreSystem.RemovePlayerProfile(conn.connectionId);
            UpdatePlayersHUD();
        }

        [Server]
        private void SendDisconnectRpcToPlayer(NetworkConnectionToClient conn)
        {
            if (conn.identity != null && conn.identity.TryGetComponent(out PlayerBehavior player))
                player.RpcDisconnect();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Infrastructure.Services.PlayerFactory;
using BH_Test_Project.Code.Runtime.Lobby;
using BH_Test_Project.Code.Runtime.Player;
using BH_Test_Project.Code.StaticData;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class GameNetworkService : NetworkRoomManager, IGameNetworkService
    {
        public event Action<NetworkConnectionToClient> OnServerReadyEvent;
        public event Action OnRoomClientEnterEvent;
        public event Action<string> OnRoomClientSceneChangedEvent;

        private readonly List<PlayerProfile> _profiles = new();
        private IPlayerFactory _playerFactory;
        private float _gameRestartDelay;
        private int _endGameScore;

        public List<NetworkRoomPlayer> PlayersInRoom => roomSlots;
        public RoomPlayer RoomPlayerPrefab => roomPlayerPrefab as RoomPlayer;
        public GameObject GamePlayerPrefab => playerPrefab;
        public int MinPlayersToStart => minPlayers;

        public void Init(IPlayerFactory playerFactory, WorldStaticData worldStaticData)
        {
            _playerFactory = playerFactory;
            _endGameScore = worldStaticData.GameEndScore;
            _gameRestartDelay = worldStaticData.GameRestartDelay;
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
            if (NetworkClient.active)
                return false;
            StartClient();
            return true;
        }

        public void LoadGameLevel()
        {
            ServerChangeScene(Constants.GAME_SCENE_NAME);
        }

        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);
            OnServerReadyEvent?.Invoke(conn);
        }

        public override void OnRoomClientEnter()
        {
            base.OnRoomClientEnter();
            OnRoomClientEnterEvent?.Invoke();
        }

        [Server]
        public void AddPlayerProfile(string playerName, int connID)
        {
            _profiles.Add(new PlayerProfile(playerName, connID));
        }

        [Server]
        public void UpdateScoreTables()
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity.TryGetComponent(out PlayerBehavior player))
                    player.TargetUpdateScoreTable(_profiles);
            }
        }

        [Server]
        public void AskForPlayerHit(int targetID, int senderID)
        {
            if (NetworkServer.connections[targetID].identity.TryGetComponent(out PlayerBehavior player))
                player.TargetPlayerHit(senderID);
        }

        [Server]
        public void SendHitSuccess(int senderID)
        {
            foreach (var profile in _profiles.Where(profile => profile.ConnectionID == senderID))
            {
                profile.IncreasePlayerScore();
                UpdateScoreTables();
                CheckEndGameConditions(profile);
                return;
            }
        }

        [Server]
        private void CheckEndGameConditions(PlayerProfile profile)
        {
            if (profile.Score >= _endGameScore)
                GameEnd(profile.PlayerName);
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

        [Server]
        private void GameEnd(string winnerName)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity.TryGetComponent(out PlayerBehavior player))
                    player.TargetGameEnd(winnerName);
            }

            StartCoroutine(RestartGameRoutine());
        }

        [Server]
        private IEnumerator RestartGameRoutine()
        {
            yield return new WaitForSeconds(_gameRestartDelay);
            ResetPlayersScore();
            ReplaceGamePlayersToRoomPlayers();
            LoadGameLevel();
        }

        private void ResetPlayersScore()
        {
            foreach (var pr in _profiles)
                pr.ResetPlayerScore();
        }

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

        private bool NullIdentity(NetworkIdentity identity) => identity == null;

        [Server]
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            if (conn.identity != null && conn.identity.TryGetComponent(out PlayerBehavior player))
                player.RpcDisconnect();
            RemovePlayerProfile(conn.connectionId);
            UpdateScoreTables();
        }

        [Server]
        private void RemovePlayerProfile(int connID)
        {
            for (int i = 0; i < _profiles.Count; i++)
                if (_profiles[i].ConnectionID == connID)
                    _profiles.RemoveAt(i);
        }
    }
}
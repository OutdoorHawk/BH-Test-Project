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
            if (NetworkClient.active || NetworkServer.active)
                return false;
            StartClient();
            return true;
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
            base.OnServerReady(conn);
            OnServerReadyEvent?.Invoke(conn);
        }

        public override void OnRoomClientEnter() // new client spawned & added to room slot
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
        public void RemovePlayerProfile(int connID)
        {
            for (int i = 0; i < _profiles.Count; i++)
                if (_profiles[i].ConnectionID == connID)
                    _profiles.RemoveAt(i);
        }

        [Server]
        public void UpdateScoreTables()
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity.TryGetComponent(out PlayerBehavior player))
                    player.RpcUpdateScoreTable(_profiles);
            }
        }

        [Server]
        public void AskForPlayerHit(int targetID, int senderID)
        {
            if (NetworkServer.connections[targetID].identity.TryGetComponent(out PlayerBehavior player))
                player.RpcPlayerHit(senderID);
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
            ServerChangeScene(GameplayScene);
        }

        [Client]
        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation,
            bool customHandling)
        {
            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
            OnRoomClientSceneChangedEvent?.Invoke(newSceneName);
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
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            RemovePlayerProfile(conn.connectionId);
            UpdateScoreTables();
        }

        private void OnGameRestarted(GameRestartMessage msg)
        {
            ServerChangeScene(GameplayScene);
        }

        private bool NullIdentity(NetworkIdentity identity) => identity == null;


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
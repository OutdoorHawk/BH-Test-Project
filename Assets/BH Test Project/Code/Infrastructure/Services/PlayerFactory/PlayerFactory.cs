using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Services.SceneContext;
using BH_Test_Project.Code.Infrastructure.Services.StaticData;
using BH_Test_Project.Code.Runtime.Lobby;
using BH_Test_Project.Code.Runtime.Player;
using BH_Test_Project.Code.StaticData;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services.PlayerFactory
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneContextService _sceneContextService;
        private List<Transform> _spawnPoints;

        public PlayerFactory(IStaticDataService staticDataService, ISceneContextService sceneContextService)
        {
            _staticDataService = staticDataService;
            _sceneContextService = sceneContextService;
        }

        [Server]
        public RoomPlayer CreateRoomPlayer(NetworkConnectionToClient conn, RoomPlayer roomPlayer)
        {
            RoomPlayer spawnedPlayer = Object.Instantiate(roomPlayer);
            NetworkServer.AddPlayerForConnection(conn, spawnedPlayer.gameObject);
            spawnedPlayer.netIdentity.AssignClientAuthority(conn);
            spawnedPlayer.RpcConstruct();
            spawnedPlayer.RpcInitializePlayer();
            return spawnedPlayer;
        }

        [Server]
        public PlayerBehavior CreateGamePlayer(NetworkConnectionToClient conn, GameObject gamePlayer)
        {
            _spawnPoints = _sceneContextService.GetSceneSpawnPoints();
            PlayerBehavior spawnedPlayer = SpawnGamePlayerOnServer(conn, gamePlayer);
            InitSpawnedPlayer(spawnedPlayer, conn);
            return spawnedPlayer;
        }

        [Server]
        private PlayerBehavior SpawnGamePlayerOnServer(NetworkConnectionToClient conn, GameObject gamePlayer)
        {
            Transform spawnTransform = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            Vector3 spawnPosition = spawnTransform.position;
            Quaternion spawnRotation = spawnTransform.rotation;
            PlayerBehavior spawnedPlayer = Object.Instantiate(gamePlayer, spawnPosition, spawnRotation)
                .GetComponent<PlayerBehavior>();
            NetworkServer.ReplacePlayerForConnection(conn, spawnedPlayer.gameObject);
            _spawnPoints.Remove(spawnTransform);
            RecollectSpawnPoints();
            return spawnedPlayer;
        }

        [Server]
        private void RecollectSpawnPoints()
        {
            if (_spawnPoints.Count == 0)
                _spawnPoints = _sceneContextService.GetSceneSpawnPoints();
        }

        [Server]
        private void InitSpawnedPlayer(PlayerBehavior spawnedPlayer, NetworkConnectionToClient conn)
        {
            PlayerStaticData staticData = _staticDataService.GetPlayerStaticData();
            WorldStaticData worldStaticData = _staticDataService.GetWorldStaticData();
            spawnedPlayer.RpcConstruct(staticData, worldStaticData);
            spawnedPlayer.RpcInitializePlayer();
        }
    }
}
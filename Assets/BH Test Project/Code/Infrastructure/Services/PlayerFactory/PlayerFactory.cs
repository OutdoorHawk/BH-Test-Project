using BH_Test_Project.Code.Infrastructure.Services.UI;
using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Runtime.Lobby;
using BH_Test_Project.Code.Runtime.Player;
using BH_Test_Project.Code.StaticData;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services.PlayerFactory
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IUIFactory _uiFactory;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;

        public PlayerFactory(IUIFactory uiFactory, IGameStateMachine gameStateMachine,
            IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
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
            PlayerBehavior spawnedPlayer = SpawnGamePlayerOnServer(conn, gamePlayer);
            InitSpawnedPlayer(spawnedPlayer);
            return spawnedPlayer;
        }

        [Server]
        private PlayerBehavior SpawnGamePlayerOnServer(NetworkConnectionToClient conn, GameObject gamePlayer)
        {
            Vector3 spawnPosition = NetworkManager.singleton.GetStartPosition().position;
            Quaternion spawnRotation = NetworkManager.singleton.GetStartPosition().rotation;
            PlayerBehavior spawnedPlayer = Object.Instantiate(gamePlayer, spawnPosition, spawnRotation)
                .GetComponent<PlayerBehavior>();
            NetworkServer.ReplacePlayerForConnection(conn, spawnedPlayer.gameObject);
            return spawnedPlayer;
        }

        [Server]
        private void InitSpawnedPlayer(PlayerBehavior spawnedPlayer)
        {
            PlayerStaticData staticData = _staticDataService.GetPlayerStaticData();
            spawnedPlayer.RpcConstruct(staticData);
            spawnedPlayer.RpcInitializePlayer();
        }
    }
}
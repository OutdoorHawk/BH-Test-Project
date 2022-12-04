using System;
using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class GameNetworkManager : NetworkRoomManager, INetworkManagerService
    {
        public event Action<NetworkConnectionToClient> OnServerReadyEvent;
        public event Action OnRoomClientEnterEvent;

        public static Dictionary<int, string> PlayerNames { get; } = new();
        public int MinPlayersToStart => minPlayers;
        public RoomPlayer RoomPlayerPrefab => roomPlayerPrefab as RoomPlayer;
        public List<NetworkRoomPlayer> PlayersInRoom => roomSlots;
        
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
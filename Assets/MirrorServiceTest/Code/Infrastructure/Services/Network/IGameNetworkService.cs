using System;
using System.Collections.Generic;
using Mirror;
using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Runtime.Lobby;

namespace MirrorServiceTest.Code.Infrastructure.Services.Network
{
    public interface IGameNetworkService : IService
    {
        event Action<NetworkConnectionToClient> OnServerReadyEvent;
        event Action OnRoomClientEnterEvent;
        event Action<string> OnRoomClientSceneChangedEvent;

        List<NetworkRoomPlayer> PlayersInRoom { get; }
        RoomPlayer RoomPlayerPrefab { get; }
        int MinPlayersToStart { get; }

        bool CreateLobbyAsHost();
        bool JoinLobbyAsClient(string address);
        void AddPlayerProfile(string playerName, int connID);
        void StopServer();
        void LoadGameLevel();
        void UpdatePlayersHUD();

        void SendHitToPlayer(int targetID, int senderID);
        void SendHitSuccess(int senderID);
    }
}
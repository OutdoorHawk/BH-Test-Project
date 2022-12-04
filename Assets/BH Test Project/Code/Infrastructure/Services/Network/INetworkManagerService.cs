using System;
using System.Collections.Generic;
using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Services.Network
{
    public interface INetworkManagerService : IService
    {
        event Action<NetworkConnectionToClient> OnServerReadyEvent;
        event Action OnRoomClientEnterEvent;

        void CreateLobbyAsHost();
        void JoinLobbyAsClient(string address);
        void StopServer();
        int MinPlayersToStart { get; }
        RoomPlayer RoomPlayerPrefab { get; }
        List<NetworkRoomPlayer> PlayersInRoom { get; }
    }
}
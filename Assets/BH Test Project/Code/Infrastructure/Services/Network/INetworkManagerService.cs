using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Services.Network
{
    public interface INetworkManagerService: IService
    {
        void CreateLobbyAsHost();
        void JoinLobbyAsClient(string address);
        RoomPlayer RoomPlayerPrefab { get; }
        RoomPlayer GetPlayerForConnection(NetworkConnectionToClient conn);
    }
}

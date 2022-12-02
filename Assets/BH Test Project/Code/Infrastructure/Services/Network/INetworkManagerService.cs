using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Runtime.Lobby;

namespace BH_Test_Project.Code.Infrastructure.Services.Network
{
    public interface INetworkManagerService: IService
    {
        RoomPlayer RoomPlayerPrefab { get; }
        void CreateLobbyAsHost();
        void JoinLobbyAsClient(string address);
    }
}

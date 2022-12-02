using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Runtime.Lobby;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Services.PlayerFactory
{
    public interface IPlayerFactory : IService
    {
        RoomPlayer CreateRoomPlayer(NetworkConnectionToClient conn);
    }
}
using BH_Test_Project.Code.Infrastructure.DI;

namespace BH_Test_Project.Code.Infrastructure.Services.Network
{
    public interface INetworkManagerService: IService
    {
        void CreateLobbyAsHost();
        void JoinLobbyAsClient(string address);
    }
}

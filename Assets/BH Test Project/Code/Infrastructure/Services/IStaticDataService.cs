using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.StaticData;
using BH_Test_Project.Code.StaticData;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Services
{
    public interface IStaticDataService : IService
    {
        void Load();
        WindowConfig GetWindow(WindowID id);
        GameNetworkManager GetLobbyNetworkManager();
        NetworkPlayerSystem GetPlayerNetworkSystem();
        PlayerStaticData GetPlayerStaticData();
        WorldStaticData GetWorldStaticData();
    }
}
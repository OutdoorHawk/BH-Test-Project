using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Infrastructure.Network;
using BH_Test_Project.Code.Infrastructure.StaticData;
using BH_Test_Project.Code.StaticData;

namespace BH_Test_Project.Code.Infrastructure.Services
{
    public interface IStaticDataService : IService
    {
        void Load();
        WindowConfig GetWindow(WindowID id);
        GameNetworkService GetLobbyNetworkManager();
        PlayerStaticData GetPlayerStaticData();
        WorldStaticData GetWorldStaticData();
    }
}
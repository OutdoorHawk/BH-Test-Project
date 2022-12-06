using MirrorServiceTest.Code.Infrastructure.Data;
using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Infrastructure.Services.Network;
using MirrorServiceTest.Code.Infrastructure.StaticData;
using MirrorServiceTest.Code.StaticData;

namespace MirrorServiceTest.Code.Infrastructure.Services.StaticData
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
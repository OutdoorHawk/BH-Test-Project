using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.DI;

namespace BH_Test_Project.Code.Infrastructure.Services
{
    public interface IUIFactory: IService
    {
        void CreateWindow(WindowID id);
        void CreateUiRoot();
    }
}
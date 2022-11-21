using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Runtime.MainMenu.Windows;

namespace BH_Test_Project.Code.Infrastructure.Services
{
    public interface IUIFactory: IService
    {
        MainMenuWindow CreateMainMenuWindow();
        void CreateUiRoot();
    }
}
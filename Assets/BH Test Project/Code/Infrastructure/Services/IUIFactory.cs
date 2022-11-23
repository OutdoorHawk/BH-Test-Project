using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Runtime.Lobby;
using BH_Test_Project.Code.Runtime.MainMenu.Windows;
using BH_Test_Project.Code.Runtime.Player.UI;

namespace BH_Test_Project.Code.Infrastructure.Services
{
    public interface IUIFactory: IService
    {
        MainMenuWindow CreateMainMenuWindow();
        LobbyMenuWindow CreateLobbyMenuWindow();
        PlayerHUD CreatePlayerHUD();
        void CreateUiRoot();
        void ClearUIRoot();
    }
}
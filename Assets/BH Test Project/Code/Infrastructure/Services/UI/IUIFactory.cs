using BH_Test_Project.Code.Infrastructure.DI;
using BH_Test_Project.Code.Runtime.Lobby;
using BH_Test_Project.Code.Runtime.MainMenu.Windows;
using BH_Test_Project.Code.Runtime.Player.UI;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Services.UI
{
    public interface IUIFactory: IService
    {
        MainMenuWindow CreateMainMenuWindow();
        LobbyMenuWindow CreateLobbyMenuWindow();
        PlayerHUD CreatePlayerHUD(NetworkConnectionToClient networkConnectionToClient);
        void CreateUiRoot();
        void ClearUIRoot();
    }
}
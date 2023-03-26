using Mirror;
using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Runtime.Lobby;
using MirrorServiceTest.Code.Runtime.MainMenu.Windows;
using MirrorServiceTest.Code.Runtime.Player.UI;
using MirrorServiceTest.Code.Runtime.Player.UI.TimeControlMenu;

namespace MirrorServiceTest.Code.Infrastructure.Services.UI
{
    public interface IUIFactory: IService
    {
        MainMenuWindow CreateMainMenuWindow();
        LobbyMenuWindow CreateLobbyMenuWindow();
        PlayerHUD CreatePlayerHUD(NetworkConnectionToClient conn);
        TimeControlHUD CreateTimeControl(NetworkConnectionToClient conn);
        void CreateUiRoot();
        void ClearUIRoot();
    }
}
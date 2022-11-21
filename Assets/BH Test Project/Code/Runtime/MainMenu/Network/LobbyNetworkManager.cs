using Mirror;

namespace BH_Test_Project.Code.Runtime.MainMenu.Network
{
    public class LobbyNetworkManager : NetworkRoomManager
    {
        public void StartGameAsHost()
        {
            if (!NetworkServer.active)
                StartHost();
            //ServerChangeScene(RoomScene);
            //SceneManager.LoadScene(RoomScene);
        }

        public void StartGameAsClient(string address)
        {
            networkAddress = address;
            if (!NetworkClient.active && !NetworkServer.active)
                StartClient();
            // ServerChangeScene(RoomScene);
            // SceneManager.LoadScene(RoomScene);
            //  OnClientChangeScene(RoomScene,SceneOperation.Normal, false);
        }
    }
}
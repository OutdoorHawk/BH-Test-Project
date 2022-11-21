using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Infrastructure.StateMachine.States;
using Mirror;

namespace BH_Test_Project.Code.Runtime.MainMenu.Network
{
    public class GameNetworkManager : NetworkRoomManager
    {
        private IGameStateMachine _gameStateMachine;

        public void Init(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void StartGameAsHost()
        {
            if (!NetworkServer.active)
                StartHost();
        }

        public void StartGameAsClient(string address)
        {
            networkAddress = address;
            if (!NetworkClient.active && !NetworkServer.active)
                StartClient();
        }

        public override void OnRoomServerPlayersReady()
        {
            base.OnRoomServerPlayersReady();
            _gameStateMachine.Enter<GameLoopState>();
        }
        
    }
}
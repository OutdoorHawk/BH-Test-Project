using BH_Test_Project.Code.Infrastructure.Services;
using BH_Test_Project.Code.Infrastructure.Services.Network;
using BH_Test_Project.Code.Runtime.Lobby;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.StateMachine.States
{
    public class LobbyState : IState, IPayloadedState<NetworkDataPayload>
    {
        private readonly IUIFactory _uiFactory;
        private readonly INetworkManagerService _networkManagerService;

        private LobbyMenuWindow _lobbyMenuWindow;
        private NetworkDataPayload _payload;

        public LobbyState(IUIFactory uiFactory, INetworkManagerService networkManagerService)
        {
            _uiFactory = uiFactory;
            _networkManagerService = networkManagerService;
        }

        public void Enter()
        {
         
        }

        public void Enter(NetworkDataPayload payload)
        {
            _payload = payload;
        }

        public void Exit()
        {
            _uiFactory.ClearUIRoot();
        }
    }
}
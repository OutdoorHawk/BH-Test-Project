using Mirror;
using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Runtime.Lobby;
using MirrorServiceTest.Code.Runtime.Player;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.Services.PlayerFactory
{
    public interface IPlayerFactory : IService
    {
        RoomPlayer CreateRoomPlayer(NetworkConnectionToClient conn, RoomPlayer roomPlayer);
        PlayerBehavior CreateGamePlayer(NetworkConnectionToClient conn, GameObject gamePlayer);
    }
}
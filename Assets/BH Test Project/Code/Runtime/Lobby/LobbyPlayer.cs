namespace BH_Test_Project.Code.Runtime.Lobby
{
    public struct LobbyPlayer
    {
        public ulong Id;
        public string PlayerName;
        public bool IsReady;

        public LobbyPlayer(ulong id, string playerName, bool isReady)
        {
            Id = id;
            PlayerName = playerName;
            IsReady = isReady;
        }
    }
}

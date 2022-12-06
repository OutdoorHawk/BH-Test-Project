using System;
using System.Collections.Generic;
using System.Linq;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.Player;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Services.Network
{
    public class ServerScoreSystem
    {
        private readonly List<PlayerProfile> _profiles = new();
        private readonly int _endGameScore;

        public ServerScoreSystem(int endGameScore)
        {
            _endGameScore = endGameScore;
        }

        [Server]
        public void AddProfileToServer(string playerName, int connID)
        {
            _profiles.Add(new PlayerProfile(playerName, connID));
        }

        [Server]
        public void SendUpdateHUDRpc()
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity != null && conn.identity.TryGetComponent(out PlayerBehavior player))
                    player.TargetUpdateHUD(_profiles);
            }
        }

        [Server]
        public void SendHitPlayerRpc(int targetID, int senderID)
        {
            if (NetworkServer.connections[targetID].identity.TryGetComponent(out PlayerBehavior player))
                player.TargetPlayerHit(senderID);
        }

        [Server]
        public void SendHitSuccessRpc(int senderID)
        {
            foreach (var profile in _profiles.Where(profile => profile.ConnectionID == senderID))
            {
                profile.IncreasePlayerScore();
                SendUpdateHUDRpc();
                return;
            }
        }

        [Server]
        public void CheckEndScoreReached(Action<string> OnGameEnded)
        {
            foreach (var profile in _profiles)
            {
                if (profile.Score >= _endGameScore)
                    OnGameEnded?.Invoke(profile.PlayerName);
            }
        }

        [Server]
        public void SendEndGameRpc(string winnerName)
        {
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity.TryGetComponent(out PlayerBehavior player))
                    player.TargetGameEnd(winnerName);
            }
        }

        [Server]
        public void ResetPlayersScore()
        {
            foreach (var pr in _profiles)
                pr.ResetPlayerScore();
        }

        [Server]
        public void RemovePlayerProfile(int connID)
        {
            for (int i = 0; i < _profiles.Count; i++)
            {
                if (_profiles[i].ConnectionID == connID)
                    _profiles.RemoveAt(i);
            }
        }
    }
}
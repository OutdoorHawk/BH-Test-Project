using System.Collections.Generic;
using BH_Test_Project.Code.Runtime.Player;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Network
{
    public class NetworkPlayerSystem
    {
        private readonly List<Player> _players;

        public NetworkPlayerSystem()
        {
            _players = new List<Player>();
        }
        
        public void AddNewPlayerToList(Player player)
        {
            _players.Add(player);
        }

        public void CheckPlayersLeft()
        {
            Debug.Log("was players: " +_players.Count);
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i] == null) 
                    _players.Remove(_players[i]);
            }
            Debug.Log("now players: " +_players.Count);
        }
    }
}
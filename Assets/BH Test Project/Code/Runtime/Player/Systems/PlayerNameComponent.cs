using System;
using Mirror;

namespace BH_Test_Project.Code.Runtime.Player.Systems
{
    public class PlayerNameComponent : NetworkBehaviour
    {
        public event Action<string> OnNameChanged;

        [SyncVar(hook = nameof(PlayerNameChanged))] private string _playerName;

        private void Start()
        {
            OnNameChanged?.Invoke(_playerName);
        }
        
        public void SetPlayerName(string playerName)
        {
            _playerName = playerName;
        }

        public string GetPlayerName()
        {
            return _playerName;
        }

        private void PlayerNameChanged(string oldValue, string newValue)
        {
            OnNameChanged?.Invoke(newValue);
        }
    }
}
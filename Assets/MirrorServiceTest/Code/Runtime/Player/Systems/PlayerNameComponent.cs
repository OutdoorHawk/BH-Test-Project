using System;
using Mirror;

namespace MirrorServiceTest.Code.Runtime.Player.Systems
{
    public class PlayerNameComponent : NetworkBehaviour
    {
        public event Action<string> OnNameChanged;

        [SyncVar(hook = nameof(PlayerNameChanged))] private string _playerName;

        private void Start()
        {
            OnNameChanged?.Invoke(_playerName);
        }

        [Command]
        public void CmdSetPlayerName(string playerName)
        {
            _playerName = playerName;
        }

        public string GetPlayerName()
        {
            return _playerName;
        }

        [Command(requiresAuthority = false)]
        private void PlayerNameChanged(string oldValue, string newValue)
        {
            OnNameChanged?.Invoke(newValue);
            gameObject.name = newValue;
        }
    }
}
using System.Collections;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player
{
    public class PlayerGameStatus
    {
        private readonly PlayerData _playerData;
        private readonly MonoBehaviour _mono;
        private readonly ColorChanger _colorChanger;

        public PlayerGameStatus(PlayerData playerData, MonoBehaviour mono)
        {
            _playerData = playerData;
            _mono = mono;
            _colorChanger = new ColorChanger(mono, playerData.PlayerHitColor);
        }

        public void PlayerHit()
        {
            _mono.StartCoroutine(PlayerHitRoutine());
        }

        private IEnumerator PlayerHitRoutine()
        {
            _colorChanger.SetPlayerHitColor();
            yield return new WaitForSeconds(_playerData.HitTime);
            _colorChanger.SetPlayerDefaultColor();
        }
    }
}
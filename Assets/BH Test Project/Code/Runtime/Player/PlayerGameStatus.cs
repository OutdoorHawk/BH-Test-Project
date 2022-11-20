using System.Collections;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player
{
    public class PlayerGameStatus
    {
        private readonly PlayerData _playerData;
        private readonly MonoBehaviour _mono;
        private readonly ColorChangerComponent _colorChangerComponent;

        public PlayerGameStatus(PlayerData playerData, MonoBehaviour mono, ColorChangerComponent changerComponent)
        {
            _playerData = playerData;
            _mono = mono;
            _colorChangerComponent = changerComponent;
        }
        
        [TargetRpc]
        public void RpcPlayerHit()
        {
            _mono.StartCoroutine(RpcPlayerHitRoutine());
        }
        
        [TargetRpc]
        private IEnumerator RpcPlayerHitRoutine()
        {
            _colorChangerComponent.CmdSetPlayerHitColor();
            yield return new WaitForSeconds(_playerData.HitTime);
            _colorChangerComponent.CmdSetPlayerDefaultColor();
        }
    }
}
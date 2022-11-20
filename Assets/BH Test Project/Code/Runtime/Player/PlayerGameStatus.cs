using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player
{
    public class PlayerGameStatus
    {
        public event Action OnHitEnded;
        
        private readonly PlayerData _playerData;
        private readonly MonoBehaviour _mono;
        private readonly ColorChangeComponent _colorChangeComponent;

        public PlayerGameStatus(PlayerData playerData, MonoBehaviour mono, ColorChangeComponent changeComponent)
        {
            _playerData = playerData;
            _mono = mono;
            _colorChangeComponent = changeComponent;
        }
        
        [TargetRpc]
        public void RpcPlayerHit()
        {
            _mono.StartCoroutine(RpcPlayerHitRoutine());
        }
        
        [TargetRpc]
        private IEnumerator RpcPlayerHitRoutine()
        {
            _colorChangeComponent.CmdSetPlayerHitColor();
            yield return new WaitForSeconds(_playerData.HitTime);
            _colorChangeComponent.CmdSetPlayerDefaultColor();
            OnHitEnded?.Invoke();
        }
    }
}
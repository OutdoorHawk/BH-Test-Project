using System.Collections;
using BH_Test_Project.Code.StaticData;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player.Systems
{
    public class PlayerGameStatus
    {
        private readonly PlayerStaticData _playerStaticData;
        private readonly MonoBehaviour _mono;
        private readonly ColorChangeComponent _colorChangeComponent;

        public bool IsHitNow { get; private set; }

        public PlayerGameStatus(PlayerStaticData playerStaticData, MonoBehaviour mono, ColorChangeComponent changeComponent)
        {
            _playerStaticData = playerStaticData;
            _mono = mono;
            _colorChangeComponent = changeComponent;
        }
        
        [TargetRpc]
        public void TargetPlayerHit()
        {
            _mono.StartCoroutine(PlayerHitRoutine());
        }
        
        private IEnumerator PlayerHitRoutine()
        {
            _colorChangeComponent.CmdSetPlayerHitColor();
            IsHitNow = true;
            yield return new WaitForSeconds(_playerStaticData.HitTime);
            _colorChangeComponent.CmdSetPlayerDefaultColor();
            IsHitNow = false;
        }
    }
}
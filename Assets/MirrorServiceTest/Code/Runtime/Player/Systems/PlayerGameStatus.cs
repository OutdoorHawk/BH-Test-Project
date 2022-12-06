using System;
using System.Collections;
using Mirror;
using MirrorServiceTest.Code.StaticData;
using UnityEngine;

namespace MirrorServiceTest.Code.Runtime.Player.Systems
{
    public class PlayerGameStatus
    {
        public event Action<NetworkIdentity> OnPlayerHit;

        private readonly PlayerStaticData _playerStaticData;
        private readonly MonoBehaviour _mono;
        private readonly ColorChangeComponent _colorChangeComponent;

        public bool IsHitNow { get; private set; }

        public PlayerGameStatus(PlayerStaticData playerStaticData, MonoBehaviour mono,
            ColorChangeComponent changeComponent)
        {
            _playerStaticData = playerStaticData;
            _mono = mono;
            _colorChangeComponent = changeComponent;
        }

        public void NotifyPlayerHit(NetworkIdentity identity) => OnPlayerHit?.Invoke(identity);

        public void PlayerHit() => _mono.StartCoroutine(PlayerHitRoutine());

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
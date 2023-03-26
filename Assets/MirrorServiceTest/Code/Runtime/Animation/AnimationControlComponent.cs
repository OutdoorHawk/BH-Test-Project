using System;
using System.Collections.Generic;
using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Infrastructure.Services.TimeControlService;
using UnityEngine;

namespace MirrorServiceTest.Code.Runtime.Animation
{
    internal sealed class AnimationControlComponent<T> : IPauseHandler where T : Enum
    {
        private Animator _animator;

        private readonly Dictionary<T, int> _parametersHashes = new();

        private TimeService _timeService;
        private T _lastTriggerType;
        private bool _isTriggered;

        public void Initialize(Animator animator)
        {
            _animator = animator;
            _timeService = DIContainer.Container.Resolve<TimeService>();
            _timeService.Register(this);
            foreach (T trigger in Enum.GetValues(typeof(T)))
            {
                _parametersHashes.Add(trigger, Animator.StringToHash(trigger.ToString()));
            }
        }

        public void SetBool(T parameter, bool value)
        {
            _animator.SetBool(_parametersHashes[parameter], value);
        }

        public void SetFloat(T parameter, float value)
        {
            _animator.SetFloat(_parametersHashes[parameter], value);
        }

        public void EnablePause()
        {
            _animator.enabled = false;
        }

        public void DisablePause()
        {
            _animator.enabled = true;
        }

        public void CleanUp()
        {
            _timeService.UnRegister(this);
        }
    }
}
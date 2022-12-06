using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirrorServiceTest.Code.Runtime.Animation
{
    internal sealed class AnimationControlComponent<T> where T : Enum
    {
        private Animator _animator;

        private readonly Dictionary<T, int> _parametersHashes = new();

        private T _lastTriggerType;
        private bool _isTriggered;

        public void Initialize(Animator animator)
        {
            _animator = animator;

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
    }
}
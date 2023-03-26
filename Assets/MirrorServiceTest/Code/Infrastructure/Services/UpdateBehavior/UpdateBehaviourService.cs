using System;
using MirrorServiceTest.Code.Infrastructure.Services.TimeControlService;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.Services.UpdateBehavior
{
    public class UpdateBehaviourService : MonoBehaviour, IUpdateBehaviourService
    {
        public event Action UpdateEvent;
        public event Action FixedUpdateEvent;
        public event Action LateUpdateEvent;
        
        private TimeService _timeService;

        private bool IsPaused => _timeService.IsPaused;

        public void Construct(TimeService timeService)
        {
            _timeService = timeService;
        }

        private void Update()
        {
            if (IsPaused)
                return;
            
            UpdateEvent?.Invoke();
        }

        private void FixedUpdate()
        {
            if (IsPaused)
                return;
            
            FixedUpdateEvent?.Invoke();
        }

        private void LateUpdate()
        {
            if (IsPaused)
                return;
            
            LateUpdateEvent?.Invoke();
        }
    }
}
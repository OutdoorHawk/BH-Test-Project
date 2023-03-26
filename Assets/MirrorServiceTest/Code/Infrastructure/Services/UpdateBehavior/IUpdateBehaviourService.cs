using System;
using MirrorServiceTest.Code.Infrastructure.DI;

namespace MirrorServiceTest.Code.Infrastructure.Services.UpdateBehavior
{
    public interface IUpdateBehaviourService: IService
    {
        public event Action UpdateEvent;
        public event Action FixedUpdateEvent;
        public event Action LateUpdateEvent;
    }
}
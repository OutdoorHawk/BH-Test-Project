using System.Collections.Generic;
using MirrorServiceTest.Code.Infrastructure.DI;

namespace MirrorServiceTest.Code.Infrastructure.Services.TimeControlService
{
    public class TimeService : IPauseHandler, IService
    {
        private readonly List<IPauseHandler> _handlers = new();
        
        public bool IsPaused { get; private set; }

        public void Register(IPauseHandler handler)
        {
            _handlers.Add(handler);
        }

        public void UnRegister(IPauseHandler handler)
        {
            _handlers.Remove(handler);
        }

        public void EnablePause()
        {
            foreach (IPauseHandler handler in _handlers)
            {
               // handler.
            }
        }

        public void DisablePause()
        {
        }

        public void SetPause()
        {
            
        }
    }
}
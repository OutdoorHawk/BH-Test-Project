using MirrorServiceTest.Code.Runtime.Player.StateMachine;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService.StateMachine
{
    public class NoRecordState : ITickableState
    {
        private readonly RecordingStateMachine _recordingStateMachine;

        public NoRecordState(RecordingStateMachine recordingStateMachine)
        {
            _recordingStateMachine = recordingStateMachine;
        }

        public void Enter()
        {
        
        }

        public void Tick()
        {
           
        }

        public void FixedTick()
        {
         
        }

        public void Exit()
        {
          
        }
    }
}
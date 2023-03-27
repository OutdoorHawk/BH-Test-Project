using MirrorServiceTest.Code.Runtime.Player.StateMachine;
using MirrorServiceTest.Code.Runtime.Player.UI.TimeControlMenu;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService.StateMachine
{
    public class NoRecordState : ITickableState
    {
        private readonly RecordingStateMachine _recordingStateMachine;
        private readonly TimeControlHUD _timeControlHUD;

        public NoRecordState(RecordingStateMachine recordingStateMachine, TimeControlHUD timeControlHUD)
        {
            _recordingStateMachine = recordingStateMachine;
            _timeControlHUD = timeControlHUD;
        }

        public void Enter()
        {
            _timeControlHUD.OnPlayPressed += ResumeRecord;
        }

        private void ResumeRecord()
        {
            _recordingStateMachine.Enter<SaveRecordState>();
        }

        public void Tick()
        {
           
        }

        public void FixedTick()
        {
         
        }

        public void Exit()
        {
            _timeControlHUD.OnPlayPressed -= ResumeRecord;
        }
    }
}
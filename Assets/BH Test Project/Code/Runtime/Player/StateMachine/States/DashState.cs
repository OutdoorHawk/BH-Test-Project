using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.Player.Movement;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine.States
{
    public class DashState : ITickableState
    {
        private readonly PlayerStateMachine _stateMachine;
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerAnimator _playerAnimator;

        public DashState(PlayerStateMachine stateMachine, PlayerMovement playerMovement,
            PlayerAnimator playerAnimator)
        {
            _playerMovement = playerMovement;
            _playerAnimator = playerAnimator;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _playerMovement.PerformDash(OnDashFinished);
            _playerAnimator.SetPlayerSpeed(0);
            _playerAnimator.PlayDashAnimation();
        }

        public void Tick()
        {
           _playerMovement.Tick();
        }

        private void OnDashFinished()
        {
            _stateMachine.Enter<BasicMovementState>();
        }

        public void Exit()
        {
            _playerAnimator.StopDashAnimation();
        }
    }
}
using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.Movement;
using UnityEngine.InputSystem;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine.States
{
    public class DashState : IState
    {
        private readonly PlayerStateMachine _stateMachine;
        private readonly PlayerMovement _playerMovement;

        public DashState(PlayerStateMachine stateMachine, PlayerMovement playerMovement,
            PlayerAnimator playerAnimator)
        {
            _playerMovement = playerMovement;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _playerMovement.PerformDash(OnDashFinished);
        }

        public void Tick()
        {
   
        }

        private void OnDashFinished()
        {
            _stateMachine.ChangeState<BasicMovementState>();
        }

        public void Exit()
        {
           
        }
    }
}
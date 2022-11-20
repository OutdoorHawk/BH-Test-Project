using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.Movement;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine.States
{
    public class BasicMovementState : ITickableState
    {
        private readonly IPlayerInput _playerInput;
        private readonly IPlayerStateMachine _stateMachine;
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerAnimator _playerAnimator;

        public BasicMovementState(IPlayerStateMachine stateMachine, PlayerMovement playerMovement,
            PlayerAnimator playerAnimator, PlayerInput playerInput)
        {
            _stateMachine = stateMachine;
            _playerMovement = playerMovement;
            _playerAnimator = playerAnimator;
            _playerInput = playerInput;
        }

        public void Enter()
        {
            _playerInput.EnableAllInput();
            _playerInput.OnDashPressed += ActivateDash;
        }

        public void Tick()
        {
            Vector2 currentInput = _playerInput.Movement.ReadValue<Vector2>();
            _playerMovement.UpdateInput(currentInput);
            _playerMovement.Tick();
            _playerAnimator.SetPlayerSpeed(_playerMovement.GetPlayerSpeed());
        }

        private void ActivateDash()
        {
            _stateMachine.Enter<DashState>();
        }

        public void Exit()
        {
            _playerInput.OnDashPressed -= ActivateDash;
            _playerInput.DisableMovementInput();
        }
    }
}
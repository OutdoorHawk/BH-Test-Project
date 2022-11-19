using BH_Test_Project.Code.Infrastructure.StateMachine;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.Movement;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine.States
{
    public class BasicMovementState : ITickableState
    {
        private readonly IPlayerInput _playerInput;
        private readonly PlayerStateMachine _stateMachine;
        private readonly PlayerMovement _playerMovement;

        public BasicMovementState(PlayerStateMachine stateMachine, PlayerMovement playerMovement,
            PlayerInput playerInput)
        {
            _stateMachine = stateMachine;
            _playerMovement = playerMovement;
            _playerInput = playerInput;
        }

        public void Enter()
        {
            _playerInput.OnDashPressed += ActivateDash;
        }

        public void Tick()
        {
            Vector2 currentInput = _playerInput.Movement.ReadValue<Vector2>();
            _playerMovement.Tick(currentInput);
        }

        private void ActivateDash()
        {
            _stateMachine.Enter<DashState>();
        }

        public void Exit()
        {
            _playerInput.OnDashPressed -= ActivateDash;
        }
    }
}
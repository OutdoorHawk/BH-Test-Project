using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.Movement;
using BH_Test_Project.Code.Runtime.Player.StateMachine.States;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine
{
    public class HitState : ITickableState
    {
        private readonly IPlayerStateMachine _playerStateMachine;
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerAnimator _playerAnimator;
        private readonly PlayerGameStatus _playerGameStatus;
        private readonly PlayerInput _playerInput;

        public HitState(IPlayerStateMachine playerStateMachine, PlayerMovement playerMovement,
            PlayerAnimator playerAnimator, PlayerGameStatus playerGameStatus, PlayerInput playerInput)
        {
            _playerStateMachine = playerStateMachine;
            _playerMovement = playerMovement;
            _playerAnimator = playerAnimator;
            _playerGameStatus = playerGameStatus;
            _playerInput = playerInput;
        }

        public void Enter()
        {
            _playerInput.EnableAllInput();
            _playerAnimator.PlayHitAnimation();
            _playerGameStatus.TargetPlayerHit();
            _playerGameStatus.OnHitEnded += EndHitState;
        }

        public void Tick()
        {
            Vector2 currentInput = _playerInput.Movement.ReadValue<Vector2>();
            float currentSpeed = _playerMovement.GetPlayerSpeed();
            _playerMovement.UpdateInput(currentInput);
            _playerMovement.Tick();
            _playerAnimator.SetPlayerSpeed(currentSpeed);
        }

        private void EndHitState()
        {
            if (_playerStateMachine.ActiveState is not EndGameState)
                _playerStateMachine.Enter<BasicMovementState>();
        }

        public void Exit()
        {
            _playerInput.DisableAllInput();
            _playerGameStatus.OnHitEnded -= Exit;
        }
    }
}
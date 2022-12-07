using MirrorServiceTest.Code.Runtime.Animation;
using MirrorServiceTest.Code.Runtime.Player.Input;
using MirrorServiceTest.Code.Runtime.Player.Systems;
using UnityEngine;

namespace MirrorServiceTest.Code.Runtime.Player.StateMachine.States
{
    public class EndGameState : ITickableState
    {
        private readonly IPlayerInput _playerInput;
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerAnimator _playerAnimator;

        public EndGameState(IPlayerInput playerInput, PlayerMovement playerMovement, PlayerAnimator playerAnimator)
        {
            _playerInput = playerInput;
            _playerMovement = playerMovement;
            _playerAnimator = playerAnimator;
        }

        public void Enter()
        {
            _playerInput.DisableDash();
        }

        public void Exit()
        {
        }

        public void Tick()
        {
            Vector2 currentInput = _playerInput.Movement.ReadValue<Vector2>();
            _playerMovement.UpdateInput(currentInput);
            _playerMovement.Tick();
            _playerAnimator.SetPlayerSpeed(_playerMovement.GetNormalizedPlayerSpeed());
        }
    }
}
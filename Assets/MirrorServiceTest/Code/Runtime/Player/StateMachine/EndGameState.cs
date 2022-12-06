using MirrorServiceTest.Code.Runtime.Animation;
using MirrorServiceTest.Code.Runtime.Player.Input;
using MirrorServiceTest.Code.Runtime.Player.Systems;
using UnityEngine;

namespace MirrorServiceTest.Code.Runtime.Player.StateMachine
{
    public class EndGameState : ITickableState
    {
        private readonly IPlayerStateMachine _playerStateMachine;
        private readonly IPlayerInput _playerInput;
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerAnimator _playerAnimator;

        public EndGameState(IPlayerStateMachine playerStateMachine, IPlayerInput playerInput,
            PlayerMovement playerMovement, PlayerAnimator playerAnimator)
        {
            _playerStateMachine = playerStateMachine;
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
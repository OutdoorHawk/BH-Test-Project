using BH_Test_Project.Code.Runtime.Player.Input;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine.States
{
    public class BasicMovementState : IState
    {
        private readonly IPlayerInput _playerInput;

        public BasicMovementState(IPlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        public void Enter()
        {
            _playerInput.EnableAllInput();
        }

        public void Tick()
        {
        }

        public void Exit()
        {
            _playerInput.DisableAllInput();
        }
    }
}
using BH_Test_Project.Code.Runtime.Player.Input;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine
{
    public class EndGameState : ITickableState
    {
        private readonly IPlayerStateMachine _playerStateMachine;
        private readonly PlayerInput _playerInput;

        public EndGameState(IPlayerStateMachine playerStateMachine, PlayerInput playerInput)
        {
            _playerStateMachine = playerStateMachine;
            _playerInput = playerInput;
        }

        public void Enter()
        {
            _playerInput.DisableAllInput();
        }

        public void Exit()
        {
        }

        public void Tick()
        {
            
        }
    }
}
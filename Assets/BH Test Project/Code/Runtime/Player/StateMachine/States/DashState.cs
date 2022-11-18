using BH_Test_Project.Code.Runtime.Player.Input;
using UnityEngine.InputSystem;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine.States
{
    public class DashState : IState
    {
        private readonly IPlayerInput _playerInput;

        public DashState(IPlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        public void Enter()
        {
            
        }

        public void Tick()
        {
   
        }

        public void Exit()
        {
           
        }
    }
}
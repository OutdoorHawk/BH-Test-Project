using System;
using UnityEngine.InputSystem;

namespace BH_Test_Project.Code.Runtime.Player.Input
{
    public interface IPlayerInput
    {
        InputAction Movement { get; }
        InputAction MouseAxis { get; }
        
        event Action OnDashPressed;
        void EnableAllInput();
        void DisableAllInput();
        void DisableMovementInput();
    }
}
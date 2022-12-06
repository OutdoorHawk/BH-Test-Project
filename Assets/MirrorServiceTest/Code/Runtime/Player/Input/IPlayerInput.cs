using System;
using UnityEngine.InputSystem;

namespace MirrorServiceTest.Code.Runtime.Player.Input
{
    public interface IPlayerInput
    {
        InputAction Movement { get; }
        InputAction MouseAxis { get; }
        event Action OnDashPressed;
        event Action OnEscapePressed;
        void EnableAllInput();
        void EnableDash();
        void DisableDash();
        void DisableAllInput();
        void DisableMovementInput();
        void DisableMovementAndMouseInput();
        void CleanUp();
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BH_Test_Project.Code.Runtime.Player.Input
{
    public class PlayerInput : IPlayerInput
    {
        private PlayerControls _playerInput;

        public InputAction Movement { get; private set; }
        public InputAction MouseAxis { get; private set; }

        public event Action OnDashPressed;
        public event Action OnEscapePressed;

        public void Init()
        {
            _playerInput = new PlayerControls();
            Movement = _playerInput.Player.Movement;
            MouseAxis = _playerInput.Player.MouseAxis;

            _playerInput.Player.Dash.started += (c) => OnDashPressed?.Invoke();
            _playerInput.Player.Escape.started += (c) => OnEscapePressed?.Invoke();
        }

        public void EnableAllInput()
        {
            _playerInput.Player.Movement.Enable();
            _playerInput.Player.Dash.Enable();
            MouseAxis.Enable();
        }

        public void DisableAllInput()
        {
            _playerInput.Player.Movement.Disable();
            _playerInput.Player.Dash.Disable();
            MouseAxis.Disable();
        }

        public void DisableMovementInput()
        {
            _playerInput.Player.Movement.Disable();
            _playerInput.Player.Dash.Disable();
        }
    }
}
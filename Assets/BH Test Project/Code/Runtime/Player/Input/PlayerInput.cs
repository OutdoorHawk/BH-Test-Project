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
        private event Action OnEscapePressed;

        public void Init()
        {
            _playerInput = new PlayerControls();
            Movement = _playerInput.Player.Movement;
            MouseAxis = _playerInput.Player.MouseAxis;

            _playerInput.Player.Dash.started += (c) => OnDashPressed?.Invoke();
            _playerInput.Player.Escape.started += (c) => OnEscapePressed?.Invoke();
            OnEscapePressed += ChangeCursorLock;
        }

        public void EnableAllInput()
        {
            _playerInput.Player.Escape.Enable();
            _playerInput.Player.Movement.Enable();
            _playerInput.Player.Dash.Enable();
            MouseAxis.Enable();
        }

        public void DisableAllInput()
        {
            _playerInput.Player.Movement.Disable();
            _playerInput.Player.Dash.Disable();
            _playerInput.Player.Escape.Disable();
            MouseAxis.Disable();
        }

        public void DisableMovementInput()
        {
            _playerInput.Player.Movement.Disable();
            _playerInput.Player.Dash.Disable();
        }
        
        public void DisableMovementAndMouseInput()
        {
            _playerInput.Player.Movement.Disable();
            _playerInput.Player.Dash.Disable();
            MouseAxis.Disable();
        }
        
        private void ChangeCursorLock()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                EnableAllInput();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                DisableMovementAndMouseInput();
            }
        }

        public void CleanUp()
        {
            OnEscapePressed -= ChangeCursorLock;
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MirrorServiceTest.Code.Runtime.Player.Input
{
    public class PlayerInput 
    {
        private PlayerControls _playerInput;

        public InputAction Movement { get; private set; }
        public InputAction MouseAxis { get; private set; }

        public event Action OnDashPressed;
        public event Action OnEscapePressed;
        public event Action OnTabPressed;

        public void Init()
        {
            _playerInput = new PlayerControls();
            Movement = _playerInput.Player.Movement;
            MouseAxis = _playerInput.Player.MouseAxis;

            _playerInput.Player.Dash.started += _ => OnDashPressed?.Invoke();
            _playerInput.Player.Escape.started += _ => OnEscapePressed?.Invoke();
            _playerInput.Player.Tab.started += _ => OnTabPressed?.Invoke();
            OnEscapePressed += ChangeCursorLock;
            OnTabPressed += ChangeCursorLock;
        }

        public void EnableAllInput()
        {
            _playerInput.Player.Enable();
            MouseAxis.Enable();
        }

        public void EnableDash()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                _playerInput.Player.Dash.Enable();
        }

        public void DisableDash()
        {
            _playerInput.Player.Dash.Disable();
        }

        public void DisableAllInput()
        {
            _playerInput.Player.Movement.Disable();
            _playerInput.Player.Dash.Disable();
            _playerInput.Player.Escape.Disable();
            _playerInput.Player.Tab.Disable();
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
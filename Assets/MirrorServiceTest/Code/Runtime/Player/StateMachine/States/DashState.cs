using System.Collections;
using MirrorServiceTest.Code.Runtime.Animation;
using MirrorServiceTest.Code.Runtime.Player.Input;
using MirrorServiceTest.Code.Runtime.Player.Systems;
using UnityEngine;

namespace MirrorServiceTest.Code.Runtime.Player.StateMachine.States
{
    public class DashState : ITickableState
    {
        private readonly IPlayerStateMachine _stateMachine;
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerAnimator _playerAnimator;
        private readonly PlayerCollisionDetector _playerCollisionDetector;
        private readonly PlayerGameStatus _playerGameStatus;
        private readonly IPlayerInput _playerInput;
        private readonly MonoBehaviour _monoBehaviour;
        private readonly float _dashRechargeTime;
        private GameObject _currentGameObject;
        public DashState(IPlayerStateMachine stateMachine, PlayerMovement playerMovement,
            PlayerAnimator playerAnimator, PlayerCollisionDetector playerCollisionDetector,
            PlayerGameStatus playerGameStatus,
            MonoBehaviour monoBehaviour, IPlayerInput playerInput, float dashRechargeTime)
        {
            _playerMovement = playerMovement;
            _playerAnimator = playerAnimator;
            _playerCollisionDetector = playerCollisionDetector;
            _playerGameStatus = playerGameStatus;
            _stateMachine = stateMachine;
            _monoBehaviour = monoBehaviour;
            _playerInput = playerInput;
            _dashRechargeTime = dashRechargeTime;
        }

        public void Enter()
        {
            _playerAnimator.SetPlayerSpeed(0);
            _playerMovement.PerformDash();
            _playerAnimator.PlayDashAnimation();
            _playerMovement.OnDashEnded += OnDashFinished;
            _playerCollisionDetector.OnPlayerCollided += OnObjectHit;
        }

        public void Tick()
        {
           
        }

        public void FixedTick()
        {
            _playerMovement.ApplyMovement();
        }

        private void OnObjectHit(ControllerColliderHit hit)
        {
            if (IsNotSameGameObject(hit) && hit.gameObject.TryGetComponent(out PlayerBehavior player))
            {
                _currentGameObject = hit.gameObject;
                _playerGameStatus.NotifyPlayerHit(player.netIdentity);
            }
        }

        private bool IsNotSameGameObject(ControllerColliderHit hit)
        {
            return _currentGameObject != hit.gameObject;
        }

        private void OnDashFinished()
        {
            if (_stateMachine.ActiveState is not EndGameState) 
                _stateMachine.Enter<BasicMovementState>();
        }

        public void Exit()
        {
            _monoBehaviour.StartCoroutine(DashRechargeRoutine());
            _playerMovement.StopDash();
            _playerAnimator.StopDashAnimation();
            _playerMovement.OnDashEnded -= OnDashFinished;
            _playerCollisionDetector.OnPlayerCollided -= OnObjectHit;
            _currentGameObject = null;
        }

        private IEnumerator DashRechargeRoutine()
        {
            _playerInput.DisableDash();
            yield return new WaitForSeconds(_dashRechargeTime);
            _playerInput.EnableDash();
        }
    }
}
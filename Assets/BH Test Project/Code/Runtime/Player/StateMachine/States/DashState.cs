using System.Collections;
using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.Player.Input;
using BH_Test_Project.Code.Runtime.Player.Systems;
using Mirror;
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Player.StateMachine.States
{
    public class DashState : ITickableState
    {
        private readonly IPlayerStateMachine _stateMachine;
        private readonly PlayerMovement _playerMovement;
        private readonly PlayerAnimator _playerAnimator;
        private readonly PlayerCollisionDetector _playerCollisionDetector;
        private readonly MonoBehaviour _monoBehaviour;
        private readonly PlayerInput _playerInput;
        private readonly float _dashRechargeTime;
        private GameObject _currentGameObject;
        private readonly uint _netId;

        public DashState(IPlayerStateMachine stateMachine, PlayerMovement playerMovement,
            PlayerAnimator playerAnimator, PlayerCollisionDetector playerCollisionDetector, uint netId,
            MonoBehaviour monoBehaviour, PlayerInput playerInput, float dashRechargeTime)
        {
            _playerMovement = playerMovement;
            _playerAnimator = playerAnimator;
            _playerCollisionDetector = playerCollisionDetector;
            _stateMachine = stateMachine;
            _netId = netId;
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
            _playerMovement.Tick();
        }

        private void OnObjectHit(ControllerColliderHit hit)
        {
            if (IsNotSameGameObject(hit) && hit.gameObject.TryGetComponent(out PlayerBehavior player))
            {
                _currentGameObject = hit.gameObject;
                CmdPlayerHit(player);
            }
        }

        [Command]
        private void CmdPlayerHit(PlayerBehavior playerBehavior)
        {
            PlayerAskHitMessage message = new PlayerAskHitMessage()
            {
                HitRecipientNetId = playerBehavior.netId,
                HitSenderNetId = _netId
            };

            NetworkClient.Send(message);
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
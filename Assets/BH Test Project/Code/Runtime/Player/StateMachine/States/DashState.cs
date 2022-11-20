using BH_Test_Project.Code.Infrastructure.Network.Data;
using BH_Test_Project.Code.Runtime.Animation;
using BH_Test_Project.Code.Runtime.Player.Movement;
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
        private GameObject _currentGameObject;
        private uint _netId;

        public DashState(IPlayerStateMachine stateMachine, PlayerMovement playerMovement,
            PlayerAnimator playerAnimator, PlayerCollisionDetector playerCollisionDetector, uint netId)
        {
            _playerMovement = playerMovement;
            _playerAnimator = playerAnimator;
            _playerCollisionDetector = playerCollisionDetector;
            _stateMachine = stateMachine;
            _netId = netId;
        }

        public void Enter()
        {
            _playerAnimator.SetPlayerSpeed(0);
            _playerMovement.PerformDash(OnDashFinished);
            _playerAnimator.PlayDashAnimation();
            _playerCollisionDetector.OnPlayerCollided += HitPlayer;
        }

        public void Tick()
        {
            _playerMovement.Tick();
        }

        private void HitPlayer(ControllerColliderHit hit)
        {
            if (IsNotSameGameObject(hit) && hit.gameObject.TryGetComponent(out Player player))
            {
                _currentGameObject = hit.gameObject;
                CmdPlayerHit(player);
            }
        }
        
        [Command]
        private void CmdPlayerHit(Player player)
        {
            PlayerHitMessage message = new PlayerHitMessage()
            {
                HurtPlayerNetId = player.netId,
                SuccessPlayerNetId = _netId
            };
   
            NetworkClient.Send(message);
        }

        private bool IsNotSameGameObject(ControllerColliderHit hit)
        {
            return _currentGameObject != hit.gameObject;
        }

        private void OnDashFinished()
        {
            _stateMachine.Enter<BasicMovementState>();
        }

        public void Exit()
        {
            _playerAnimator.StopDashAnimation();
            _playerCollisionDetector.OnPlayerCollided -= HitPlayer;
            _currentGameObject = null;
        }
    }
}
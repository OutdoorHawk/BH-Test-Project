using MirrorServiceTest.Code.Runtime.Player.StateMachine;
using MirrorServiceTest.Code.Runtime.Player.Systems;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService.Systems
{
    public class PlayerRecordedElements
    {
        public Rigidbody PlayerRigidbody;
        public Animator PlayerAnimator;
        public PlayerMovement PlayerMovement;
        public IPlayerStateMachine PlayerStateMachine;
    }
}
using System.Collections.Generic;
using MirrorServiceTest.Code.Runtime.Player;
using MirrorServiceTest.Code.Runtime.Player.StateMachine;
using MirrorServiceTest.Code.Runtime.Player.StateMachine.States;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService.Systems
{
    public class PlayerRecordSystem
    {
        private readonly List<PlayerRecordedElements> _players = new();

        public void AddPlayer(PlayerBehavior playerBehavior)
        {
            PlayerRecordedElements recordedElements = new PlayerRecordedElements()
            {
                PlayerRigidbody = playerBehavior.GetComponent<Rigidbody>(),
                PlayerAnimator = playerBehavior.GetComponent<Animator>(),
                PlayerMovement = playerBehavior.Movement,
                PlayerStateMachine = playerBehavior.StateMachine
            };
            _players.Add(recordedElements);
        }

        public void RecordPlayersData(FrameRecord frameRecord)
        {
            foreach (PlayerRecordedElements player in _players)
                RecordPlayer(frameRecord, player);
        }

        public void LoadPlayersData(FrameRecord frameData)
        {
            foreach (PlayerRecordedElements player in _players)
                LoadPlayer(frameData, player);
        }

        private void RecordPlayer(FrameRecord frameRecord, PlayerRecordedElements player)
        {
            frameRecord.PlayerFrameRecord.Position = player.PlayerRigidbody.position;
            frameRecord.PlayerFrameRecord.Velocity = player.PlayerRigidbody.velocity;
            frameRecord.PlayerFrameRecord.DashRemainingDistance = player.PlayerMovement.DashRemainingDistance;
            frameRecord.PlayerFrameRecord.StateMachineState = player.PlayerStateMachine.ActiveState;
            RecordAnimation(frameRecord.PlayerFrameRecord.AnimationLayers, player.PlayerAnimator);
        }

        private void RecordAnimation(AnimationLayers[] animationLayers, Animator animator)
        {
            for (int i = 0; i < animator.layerCount; i++)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(i);
                AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(i);
                AnimationClip clip = clipInfos[0].clip;
                float time = stateInfo.normalizedTime % 1;
                animationLayers[i].CurrentAnimationClip = clip;
                animationLayers[i].AnimationTimePlayed = time;
            }
        }

        private void LoadPlayer(FrameRecord frameData, PlayerRecordedElements player)
        {
            LoadStateMachineState(frameData, player.PlayerStateMachine);
            LoadAnimation(frameData.PlayerFrameRecord.AnimationLayers, player.PlayerAnimator);
            player.PlayerRigidbody.position = frameData.PlayerFrameRecord.Position;
            player.PlayerRigidbody.velocity = frameData.PlayerFrameRecord.Velocity;
            player.PlayerMovement.DashRemainingDistance = frameData.PlayerFrameRecord.DashRemainingDistance;
        }

        private void LoadAnimation(AnimationLayers[] animationLayers, Animator animator)
        {
            for (int i = 0; i < animationLayers.Length; i++)
            {
                string animationName = animationLayers[i].CurrentAnimationClip.name;
                float animationTimePlayed = animationLayers[i].AnimationTimePlayed;
                animator.Play(animationName, i, animationTimePlayed);
            }
        }

        private void LoadStateMachineState(FrameRecord frameData, IPlayerStateMachine playerPlayerStateMachine)
        {
            switch (frameData.PlayerFrameRecord.StateMachineState)
            {
                case BasicMovementState:
                    playerPlayerStateMachine.Enter<BasicMovementState>();
                    break;
                case DashState:
                    playerPlayerStateMachine.Enter<DashState>();
                    break;
                case EndGameState:
                    playerPlayerStateMachine.Enter<EndGameState>();
                    break;
            }
        }
    }
}
using UnityEngine;

namespace BH_Test_Project.Code.Runtime.Animation
{
    public class PlayerAnimator
    {
        private readonly AnimationControlComponent<PlayerAnimationParameter> _animationControlComponent;

        public PlayerAnimator(Animator animator)
        {
            _animationControlComponent = new AnimationControlComponent<PlayerAnimationParameter>();
            _animationControlComponent.Initialize(animator);
        }

        public void SetPlayerSpeed(float speed)
        {
            _animationControlComponent.SetFloat(PlayerAnimationParameter.Speed, speed);
        }

        public void PlayDashAnimation()
        {
            _animationControlComponent.SetBool(PlayerAnimationParameter.Dash, true);
        }

        public void StopDashAnimation()
        {
            _animationControlComponent.SetBool(PlayerAnimationParameter.Dash, false);
        }

        public void PlayHitAnimation()
        {
            _animationControlComponent.SetTrigger(PlayerAnimationParameter.Hit);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    [DefaultExecutionOrder(100)]
    public class AnimationController : MonoBehaviour
    {
        [Header(" Settings ")]
        private Animator animator;

        private Enemy currentEnemy;
        void Start()
        {

         animator = GetComponent<Animator>();    
        
        }
        private void OnEnable()
        {
            PlayerActivity.OnHeliCopterJump += PlayJumpToHelicopterAnim;
            LineFollower.OnCharacterStartMoving +=  PlayRunAnimation;
            LineFollower.OnCharacterReachDestination += PlayIdleAnimation;
            LineFollower.OnCharacterReachedEnemy += PlayPunchAnimation;
        }
        private void OnDestroy()
        {
            PlayerActivity.OnHeliCopterJump -= PlayJumpToHelicopterAnim;
            LineFollower.OnCharacterReachedEnemy -= PlayPunchAnimation;
            LineFollower.OnCharacterStartMoving -= PlayRunAnimation;
            LineFollower.OnCharacterReachDestination -= PlayIdleAnimation;
        }

        void PlayRunAnimation()
        {
            animator.SetTrigger("Run");
        }
        void PlayIdleAnimation()
        {
            animator.ResetTrigger("Run");
            animator.SetTrigger("Idle");
        }
        void PlayPunchAnimation(Enemy enemy)
        {
            currentEnemy = enemy;
            animator.SetTrigger("Punch");
        }

        public void AnimationPunchEventCallback()
        {
            if (currentEnemy != null)
            {
                int damageAmount = 10; 
                currentEnemy.TakeDamage(damageAmount);
            }
        }

        public void PlayJumpToHelicopterAnim()
        {
            animator.SetTrigger("JumpToHelicopter");
        }

    }
}

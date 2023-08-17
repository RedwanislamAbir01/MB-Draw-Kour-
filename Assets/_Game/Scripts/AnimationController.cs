using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public class AnimationController : MonoBehaviour
    {
        [Header(" Settings ")]
        private Animator animator;

        public static event Action<EnemyAnimationController> OnPlayerPunchEvent;
        void Start()
        {

         animator = GetComponent<Animator>();    
        
        }
        private void OnEnable()
        {
           
            LineFollower.OnCharacterStartMoving +=  PlayRunAnimation;
            LineFollower.OnCharacterReachDestination += PlayIdleAnimation;
            LineFollower.OnCharacterReachedEnemy += PlayPunchAnimation;
        }
        private void OnDestroy()
        {
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
            animator.SetTrigger("Idle");
        }
        void PlayPunchAnimation()
        {
            animator.SetTrigger("Punch");
        }

        public void AnimationPunchEventCallback(EnemyAnimationController enemy)
        {
            OnPlayerPunchEvent?.Invoke(enemy);
        }
        public void AnimationFinalHitEventCallback()
        {
            FindObjectOfType<EnemyAnimationController>().DeathAnim();
        }
    }
}

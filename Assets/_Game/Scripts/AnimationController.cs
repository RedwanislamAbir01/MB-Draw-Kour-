using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public class AnimationController : MonoBehaviour
    {
        [Header(" Settings ")]
        private Animator animator;

        void Start()
        {

         animator = GetComponent<Animator>();    
        
        }
        private void OnEnable()
        {
            LineFollower.OnCharacterStartMoving +=  PlayRunAnimation;
            LineFollower.OnCharacterReachDestination += PlayIdleAnimation;
        }
        private void OnDestroy()
        {
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
    }
}

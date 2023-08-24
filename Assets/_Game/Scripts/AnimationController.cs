using DG.Tweening;
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

        private GameObject parentObj;

        [SerializeField] private ParticleSystem _hitImpact;
        [SerializeField] private Transform _hitImpactSpawnPoint;
        
        void Start()
        {
         parentObj = transform.parent.gameObject;
         animator = GetComponent<Animator>();    
        
        }
        private void OnEnable()
        {
            PlayerActivity.OnHeliCopterJump += PlayJumpToHelicopterAnim;
            LineFollower.OnCharacterStartMoving +=  PlayRunAnimation;
            LineFollower.OnCharacterReachDestination += PlayIdleAnimation;
            LineFollower.OnCharacterReachedEnemy += PlayPunchAnimation;
            CamManager.OnCamReseting += ChildParentPosFix;
            LineFollower.OnCharacterStopped += PlayIdleAnimation;
        }
        private void OnDestroy()
        {
            CamManager.OnCamReseting -= ChildParentPosFix;
            PlayerActivity.OnHeliCopterJump -= PlayJumpToHelicopterAnim;
            LineFollower.OnCharacterReachedEnemy -= PlayPunchAnimation;
            LineFollower.OnCharacterStartMoving -= PlayRunAnimation;
            LineFollower.OnCharacterReachDestination -= PlayIdleAnimation;
            LineFollower.OnCharacterStopped -= PlayIdleAnimation;
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
            animator.applyRootMotion = true;
            currentEnemy = enemy;
            animator.SetTrigger("Punch");
            AnimationPunchEventCallback(currentEnemy);
            StartCoroutine(ResetRootMotionRoutine());
        }
        private IEnumerator ResetRootMotionRoutine()
        {  
            yield return new WaitForSeconds(1.5f);
        }
        public void AnimationPunchEventCallback(Enemy enemy)
        {
            if (currentEnemy != null)
            {
                int damageAmount = 100; 
                currentEnemy.TakeDamage(damageAmount);
            }
        }

        public void PlayJumpToHelicopterAnim()
        {
            animator.SetTrigger("JumpToHelicopter");
        }


        void ChildParentPosFix()
        {
            animator.applyRootMotion = false;

            transform.parent = null;

            Vector3 pos = new Vector3(transform.position.x, parentObj.transform.position.y, transform.position.z);

            parentObj.transform.position = pos;

            transform.parent = parentObj.transform;

            transform.DOLocalRotate(new Vector3(0, 0, 0), .1f).SetDelay(.2f);
        }

        public void PlayHitImpact()
        {
            Instantiate(_hitImpact, _hitImpactSpawnPoint.position, Quaternion.identity);
        }
    }
}

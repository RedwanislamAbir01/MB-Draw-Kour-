using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public class EnemyAnimationController : MonoBehaviour
    {
        public static event Action OnEnemyDeath;
        private Animator _animator;
        private void Start()
        {
           _animator = GetComponent<Animator>();
            AnimationController.OnPlayerPunchEvent += PlayPlayerHitAnimation;
        }

        private void OnDestroy()
        {
           
            AnimationController.OnPlayerPunchEvent -= PlayPlayerHitAnimation;
        }
        private void PlayPlayerHitAnimation(EnemyAnimationController enemy)
        {
            _animator.SetTrigger("Hit");
            print("Punched");
        }

        public void DeathAnim()
        {
            _animator.SetTrigger("Die");
            GetComponentInParent<Collider>().enabled = false;
            OnEnemyDeath?.Invoke();
        }
    }
}

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

        Enemy enemy;
        private void Start()
        {
           enemy = GetComponentInParent<Enemy>();
           _animator = GetComponent<Animator>();
           enemy.OnHit += PlayPlayerHitAnimation;
           enemy.OnDeath += DeathAnim;
           enemy.OnStartMoving += RunAnim;
           enemy.OnStopMoving += IdleAnim;
        }

        private void OnDestroy()
        {
            enemy.OnStartMoving -= RunAnim;
            enemy.OnStopMoving -= IdleAnim;
            enemy.OnDeath -= DeathAnim;
            enemy.OnHit -= PlayPlayerHitAnimation;
        }
        private void PlayPlayerHitAnimation()
        {
            _animator.SetTrigger("Hit");
        }

        private void DeathAnim()
        {
            _animator.SetTrigger("Die");
        }
        private void IdleAnim ()
        {
            _animator.SetTrigger("Idle");
        }
        private void  RunAnim()
        {
            _animator.SetTrigger("Run");
        }
    }
}

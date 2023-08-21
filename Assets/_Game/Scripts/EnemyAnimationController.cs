using System;
using UnityEngine;

namespace _Game
{
    [RequireComponent(typeof(Animator))]  // Ensures that this component requires an Animator to function properly
    public class EnemyAnimationController : MonoBehaviour
    {
        public event Action OnShoot;
        public static event Action OnEnemyDeath;

        private Animator _animator;
        private Enemy _enemy;
        [SerializeField] private PlayerDetector _playerDetector;

        private void Start()
        {
            InitializeComponents();
            HookUpEventHandlers();
        }

        private void OnDestroy()
        {
            UnhookEventHandlers();
        }

        private void InitializeComponents()
        {
            _animator = GetComponent<Animator>();
            _enemy = GetComponentInParent<Enemy>();
        }

        private void HookUpEventHandlers()
        {
            _enemy.OnHit += PlayPlayerHitAnimation;
            _enemy.OnDeath += DeathAnim;
            _enemy.OnStartMoving += RunAnim;
            _enemy.OnStopMoving += IdleAnim;
            _playerDetector.OnPlayerDetected += ShootAnim;
        }

        private void UnhookEventHandlers()
        {
            _enemy.OnHit -= PlayPlayerHitAnimation;
            _enemy.OnDeath -= DeathAnim;
            _enemy.OnStartMoving -= RunAnim;
            _enemy.OnStopMoving -= IdleAnim;
            _playerDetector.OnPlayerDetected -= ShootAnim;
        }

        private void PlayPlayerHitAnimation()
        {
            _animator.SetTrigger("Hit");
        }

        private void DeathAnim()
        {
            _animator.SetTrigger("Die");
            OnEnemyDeath?.Invoke();
        }

        private void IdleAnim()
        {
            _animator.SetTrigger("Idle");
        }

        private void RunAnim()
        {
            _animator.SetTrigger("Run");
        }

        private void ShootAnim()
        {
            _animator.SetTrigger("Shoot");
        }

        private void OnShootCallBackEvent()
        {
            OnShoot?.Invoke();
        }
    }
}

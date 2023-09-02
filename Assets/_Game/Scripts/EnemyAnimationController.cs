using System;
using UnityEngine;

namespace _Game
{
    [RequireComponent(typeof(Animator))]  // Ensures that this component requires an Animator to function properly
    public class EnemyAnimationController : MonoBehaviour
    {
        public event Action OnGotHit;
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

        private void PlayPlayerHitAnimation(string punchTriggerName)
        {
            print("on hit");
            if (punchTriggerName == "Punch")
            {
                _animator.SetTrigger("Hit");
            }
            else if (punchTriggerName == "Punch1")
            {
                _animator.SetTrigger("Hit1");
            }
            else if (punchTriggerName == "Punch2")
            {
                _animator.SetTrigger("Hit2");
            }
            if (GetComponentInChildren<EnemyGun>() != null)
            GetComponentInChildren<EnemyGun>().DropGun();
        }


        private void DeathAnim()
        {
            print("on death");
            _animator.applyRootMotion = true;

            if (_enemy.punchTriggerName == "Punch")
            {
                _animator.SetTrigger("Die");
            }
            else if (_enemy.punchTriggerName == "Punch1")
            {
                _animator.SetTrigger("Die1");
            }
            else if (_enemy.punchTriggerName == "Punch2")
            {
                _animator.SetTrigger("Die2");
            }
            // Add more conditions for other punch trigger names as needed
           
            OnEnemyDeath?.Invoke();
            if (GetComponentInChildren<EnemyGun>()!= null)
            GetComponentInChildren<EnemyGun>().DropGun();
        }

        public void GotHit()
        {
            OnGotHit?.Invoke();
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

using System;
using System.Collections;
using _Game.Managers;
using UnityEngine;

namespace _Game
{
    public class PlayerHealth : MonoBehaviour
    {
        public static event EventHandler OnDeath;
        
        [SerializeField] private Transform _rayStartPoint;
        [SerializeField] private float _rayLength = 1f;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _deathDelay = 1f;
        [SerializeField]
        private ParticleSystem _deathFx;
        private Rigidbody _rigidbody;

        private bool _isAlive;
        private static readonly int _Death = Animator.StringToHash("Death");

        private void Awake()
        {
            _isAlive = true;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if(!_isAlive) return;
            
            var ray = new Ray(_rayStartPoint.position, Vector3.down);

            if (!Physics.Raycast(ray, _rayLength, _groundLayer))
            {
                if (!PlayerState.Instance.IsDoingParkour())
                {
                    _isAlive = false;
                    StartCoroutine(DeathRoutine());
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Bullet"))
            {
                Instantiate(_deathFx, other.ClosestPoint(transform.position), Quaternion.identity);


                _isAlive = false;

                Destroy(other.gameObject);

                _animator.SetTrigger(_Death);
                
                OnDeath?.Invoke(this, EventArgs.Empty);

                GameManager.Instance.LevelFail();
            }
        }
        public bool IsAlive() => _isAlive;

        private IEnumerator DeathRoutine()
        {
            yield return new WaitForSeconds(_deathDelay);
            
            _animator.SetTrigger(_Death);
                    
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
            
            CamManager.Instance.EnableStartCam();
                    
            OnDeath?.Invoke(this, EventArgs.Empty);
            
            GameManager.Instance.LevelFail();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_rayStartPoint.position, Vector3.down * _rayLength);
            Gizmos.color = Color.white;
        }
#endif
    }
}
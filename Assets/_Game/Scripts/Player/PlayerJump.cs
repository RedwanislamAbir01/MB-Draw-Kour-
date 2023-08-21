using System;
using DG.Tweening;
using UnityEngine;

namespace _Game
{
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _visual;
        [SerializeField] private float _detectionRange = 2f;
        [SerializeField] private LayerMask _obstacleLayer;

        private bool _isJumping;
        
        private static readonly int _Jump = Animator.StringToHash("Jump");
        private static readonly int Run = Animator.StringToHash("Run");

        private void Update()
        {
            if(_isJumping) return;
            
            var ray = new Ray(transform.position, transform.forward * _detectionRange);

            if (Physics.Raycast(ray, out var hitInfo, _detectionRange, _obstacleLayer))
            {
                if (hitInfo.transform.TryGetComponent(out IJumpable jumpable))
                {
                    Jump(jumpable.JumpHeightOffset, jumpable.TotalJumpDuration);
                }
            }
        }

        private void Jump(float jumpHeightOffset, float totalJumpDuration)
        {
            _isJumping = true;
            
            _animator.SetTrigger(_Jump);

            const float duration = 0.1f;
            
            _visual.DOLocalMoveY(jumpHeightOffset, duration).OnComplete(() =>
            {
                _visual.DOLocalMoveY(0f, duration).SetDelay(totalJumpDuration).OnComplete(() =>
                {
                    _isJumping = false;
                    _animator.SetTrigger(Run);
                });
            });
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward * _detectionRange);
            Gizmos.color = Color.white;
        }
#endif
    }
}

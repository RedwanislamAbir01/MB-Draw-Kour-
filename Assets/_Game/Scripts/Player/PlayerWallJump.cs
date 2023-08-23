using DG.Tweening;
using UnityEngine;

namespace _Game
{
    public class PlayerWallJump : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private LineFollower _lineFollower;
        [SerializeField] private float _yOffset1 = 2f;
        [SerializeField] private float _yOffset2 = 4.5f;
        [SerializeField] private float _zOffset = 1f;
        [SerializeField] private float _jumpDuration1 = 0.7f;
        [SerializeField] private float _jumpDuration2 = 0.5f;
        [SerializeField] private float _jumpDelay1 = 0.5f;
        [SerializeField] private float _jumpDelay2 = 1f;

        private bool _isJumping;
        
        private static readonly int _Climb = Animator.StringToHash("Climb");
        private static readonly int _Run = Animator.StringToHash("Run");

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("WallJumpTrigger"))
            {
                if(_isJumping) return;
                
                transform.rotation = other.transform.rotation;
                Jump();
            }
        }

        private void Jump()
        {
            _isJumping = true;
            _lineFollower.ClearLine();
            _animator.ResetTrigger(_Run);
            _animator.SetTrigger(_Climb);
            PlayerState.Instance.SetState(PlayerState.State.Parkour);
            
            var targetPosition1 = transform.position + transform.up * _yOffset1;
            var targetPosition2 = transform.position + transform.forward * _zOffset + transform.up * _yOffset2;
            
            transform.DOMove(targetPosition1, _jumpDuration1).SetDelay(_jumpDelay1).OnComplete(() =>
            {
                transform.DOMove(targetPosition2, _jumpDuration2).SetDelay(_jumpDelay2).OnComplete(() =>
                {
                    _isJumping = false;
                    PlayerState.Instance.SetState(PlayerState.State.Default);
                    PlayerState.Instance.DisableMoving();
                });
            });
        }
    }
}
using DG.Tweening;
using UnityEngine;

namespace _Game
{
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] private LineFollower _lineFollower;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip[] _jumpClips;
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private Transform _visual;
        [SerializeField] private float _detectionRange = 2f;
        [SerializeField] private LayerMask _obstacleLayer;

        private bool _isJumping;
        
        private static readonly int _Jump = Animator.StringToHash("Jump");
        private static readonly int Run = Animator.StringToHash("Run");

        private void Start()
        {
            _animatorOverrideController = new AnimatorOverrideController
            {
                runtimeAnimatorController = _animator.runtimeAnimatorController
            };
        }
        
        private void Update()
        {
            if(_isJumping) return;
            
            var ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out var hitInfo, _detectionRange, _obstacleLayer))
            {
                if (hitInfo.transform.TryGetComponent(out IJumpable jumpable))
                {
                    if (_lineFollower.IsWayPointAvailable())
                    {
                        if(CheckIfPointBetweenObjects(transform, hitInfo.transform, _lineFollower.LastWayPoint()))
                        {
                            const float distance = 2.5f;
                            var newPoint = _lineFollower.LastWayPoint() + transform.forward * distance;
                            _lineFollower.AddToWayPoint(newPoint);
                        }
                        else
                        {
                            if (hitInfo.collider.bounds.Contains(_lineFollower.LastWayPoint()))
                            {
                                // Debug.Log("Inside");
                                const float distance = 1.5f;
                                var newPoint = _lineFollower.LastWayPoint() + transform.forward * distance;
                                _lineFollower.AddToWayPoint(newPoint);
                            }
                            else
                            {
                                // Debug.Log("Outside");
                                Jump(jumpable.JumpHeightOffset, jumpable.TotalJumpDuration);
                            }
                        }   
                    }
                }
            }
        }
        
        private bool CheckIfPointBetweenObjects(Transform objectA, Transform objectB, Vector3 point)
        {
            var directionVector = objectB.position - objectA.position;
            var vectorToObjectA = point - objectA.position;
            var vectorToObjectB = point - objectB.position;

            var dotProductA = Vector3.Dot(directionVector, vectorToObjectA);
            var dotProductB = Vector3.Dot(directionVector, vectorToObjectB);

            if (dotProductA > 0 && dotProductB < 0)
            {
                // Debug.Log("Point is between the objects.");
                return true;
            }

            // Debug.Log("Point is NOT between the objects.");
            return false;
        }

        private void Jump(float jumpHeightOffset, float totalJumpDuration)
        {
            _isJumping = true;
            PlayerState.Instance.SetState(PlayerState.State.Parkour);

            var randomIdx = Random.Range(0, _jumpClips.Length);
            
            _animatorOverrideController["Jumping"] = _jumpClips[randomIdx];
            _animator.runtimeAnimatorController = _animatorOverrideController;
            
            _animator.SetTrigger(_Jump);

            const float duration = 0.1f;
            
            _visual.DOLocalMoveY(jumpHeightOffset, duration).OnComplete(() =>
            {
                _visual.DOLocalMoveY(0f, duration).SetDelay(totalJumpDuration).OnComplete(() =>
                {
                    _isJumping = false;
                    PlayerState.Instance.SetState(PlayerState.State.Default);

                    if (_lineFollower.IsWayPointAvailable())
                    {
                        _animator.SetTrigger(Run);
                    }
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

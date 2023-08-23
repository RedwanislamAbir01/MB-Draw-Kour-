using DG.Tweening;
using UnityEngine;

namespace _Game
{
    public class PlayerWallRun : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private LineFollower _lineFollower;
        
        private Vector3 _targetPosition;
        private Vector3 _initialPosition;
        private bool _canMove;
        
        private static readonly int _WallRunStart = Animator.StringToHash("WallRunStart");
        private static readonly int _Run = Animator.StringToHash("Run");
        private static readonly int _WallRunStop = Animator.StringToHash("WallRunStop");

        private void Update()
        {
            if (!_canMove) return;
            
            transform.position += transform.forward * (_lineFollower.GetSpeed() * Time.deltaTime);  
            
            const float distanceThreshold = 1.5f;
            
            if (Vector3.Distance(transform.position, _targetPosition) <= distanceThreshold)
            {
                StopWallRun();  
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.CompareTag("LeftTrigger") && !_canMove)
            {
                StartWallRun(other.transform.forward, other.transform.GetChild(0).position);
            }
            
            if(other.transform.CompareTag("RightTrigger") && !_canMove)
            {
                StartWallRun(-other.transform.forward, other.transform.GetChild(0).position);
            }
        }
        
        private void StartWallRun(Vector3 direction, Vector3 targetPosition)
        {
            _lineFollower.ClearLine();
            PlayerState.Instance.SetState(PlayerState.State.Parkour);
            
            transform.forward = direction;
            _initialPosition = transform.position;
            _targetPosition = targetPosition;

            _animator.ResetTrigger(_WallRunStop);
            _animator.SetTrigger(_WallRunStart);

            const float duration = 0.5f;
            transform.DOMoveY(_targetPosition.y, duration);
            _canMove = true;
        }

        private void StopWallRun()
        {
            _animator.ResetTrigger(_Run);
            _animator.SetTrigger(_WallRunStop);
            
            const float duration = 0.5f;
            transform.DOMoveY(_initialPosition.y, duration).OnComplete(() =>
            {
                _canMove = false;
                PlayerState.Instance.SetState(PlayerState.State.Default);
                
                CamManager.Instance.EnableStartCam();
                PlayerState.Instance.DisableMoving();
            });
        }
    }
}
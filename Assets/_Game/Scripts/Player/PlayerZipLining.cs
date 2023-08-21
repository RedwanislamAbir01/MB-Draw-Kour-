using DG.Tweening;
using UnityEngine;

namespace _Game
{
    [RequireComponent(typeof(LineFollower))]
    public class PlayerZipLining : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private LineFollower _lineFollower;
        
        private static readonly int _Ziplining = Animator.StringToHash("Ziplining");
        private static readonly int _Idle = Animator.StringToHash("Idle");
        private static readonly int _Run = Animator.StringToHash("Run");

        public void ZipLine(Transform startPoint, Transform endPoint, Transform dropPoint, float duration)
        {
            _lineFollower.ClearLine();
            _animator.SetTrigger(_Ziplining);

            const float moveDuration = 0.1f;
            transform.DORotate(Vector3.zero, moveDuration);
            transform.DOMove(startPoint.position, moveDuration).OnComplete(() =>
            {
                transform.DOMove(endPoint.position, duration).SetEase(Ease.Linear).OnComplete(() =>
                {
                    _animator.ResetTrigger(_Run);
                    _animator.SetTrigger(_Idle);
                    transform.DOMove(dropPoint.position, moveDuration);
                    CamManager.Instance.EnableStartCam();
                });
            });
        }
    }
}
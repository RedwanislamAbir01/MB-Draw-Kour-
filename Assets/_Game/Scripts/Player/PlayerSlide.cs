using DG.Tweening;
using UnityEngine;

namespace _Game
{
    public class PlayerSlide : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _visual;

        [Header("Slide Settings")]
        [SerializeField] private float _detectionRange = 2f;
        [SerializeField] private LayerMask _obstacleLayer;

        private bool _isSliding;

        private static readonly int _Slide = Animator.StringToHash("Slide");
        private static readonly int _Run = Animator.StringToHash("Run");

        private void Update()
        {
            if (_isSliding)
                return;

            CheckForSlideableObstacle();
        }

        private void CheckForSlideableObstacle()
        {
            var ray = new Ray(transform.position, transform.forward);

            if (Physics.Raycast(ray, out var hitInfo, _detectionRange, _obstacleLayer))
            {
                if (hitInfo.transform.TryGetComponent(out ISlideable slideable))
                {
                    InitiateSlide();
                }
            }
        }

        private void InitiateSlide()
        {
            _isSliding = true;

            PlayerState.Instance.SetState(PlayerState.State.Parkour);

            _animator.SetTrigger(_Slide);

            SlideVisualEffect();
        }

        private void SlideVisualEffect()
        {
            const float slideDuration = 0.1f;
            const float slideDownDistance = -0.8f;
            const float slideUpDelay = .8f;
            const float slideUpDuration = 0.3f;

            _visual.DOLocalMoveY(slideDownDistance, slideDuration)
                .OnComplete(() =>
                {
                    _visual.DOLocalMoveY(0f, slideUpDuration).SetDelay(slideUpDelay)
                        .OnComplete(CompleteSlide);
                });
        }

        private void CompleteSlide()
        {
            _isSliding = false;

            PlayerState.Instance.SetState(PlayerState.State.Default);

            _animator.SetTrigger(_Run);
        }
    }
}

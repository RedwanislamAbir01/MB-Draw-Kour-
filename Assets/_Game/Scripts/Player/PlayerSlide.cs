using DG.Tweening;
using UnityEngine;

namespace _Game
{
    public class PlayerSlide : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LineFollower _lineFollower;
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
                    if (_lineFollower.IsWayPointAvailable())
                    {
                        if (CheckIfPointBetweenObjects(transform, hitInfo.transform, _lineFollower.LastWayPoint()))
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
                                InitiateSlide();
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


            private bool CanStartSlide()
            {
                // Add any additional conditions to check if sliding can start
                return true; // For example, you might want to check if the player is grounded.
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
                        PlayerState.Instance.SetState(PlayerState.State.Default);
                        _visual.DOLocalMoveY(0f, slideUpDuration).SetDelay(slideUpDelay)
                            .OnComplete(CompleteSlide);
                    });
            }

            private void CompleteSlide()
            {
                _isSliding = false;

              

            if (_lineFollower.IsWayPointAvailable())
            {
                _animator.SetTrigger(_Run);
            }
        }
        }
    }


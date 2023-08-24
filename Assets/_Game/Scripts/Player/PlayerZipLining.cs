using System;
using DG.Tweening;
using UnityEngine;

namespace _Game
{
    [RequireComponent(typeof(LineFollower))]
    public class PlayerZipLining : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private LineFollower _lineFollower;
        [SerializeField] private Vector3 _camOffset;
        [SerializeField] private GameObject _playerHandle;
        
        private static readonly int _Ziplining = Animator.StringToHash("Ziplining");
        private static readonly int _Idle = Animator.StringToHash("Idle");
        private static readonly int _Run = Animator.StringToHash("Run");

        private void Awake()
        {
            _playerHandle.SetActive(false);
        }

        public void ZipLine(Transform startPoint, Transform endPoint, Transform dropPoint, float jumpDuration, float zipDuration, GameObject startHandle, GameObject endHandle, ParticleSystem landEffect)
        {
            _lineFollower.ClearLine();
            _animator.SetTrigger(_Ziplining);
            PlayerState.Instance.SetState(PlayerState.State.Parkour);
            CamManager.Instance.SetFollowCamTrackedObjectOffset(_camOffset);
            
            const float rotationDuration = 0.1f;
            transform.DORotate(Vector3.zero, rotationDuration);
            transform.DOMove(startPoint.position, jumpDuration).OnComplete(() =>
            {
                startHandle.SetActive(false);
                _playerHandle.SetActive(true);
                
                transform.DOMove(endPoint.position, zipDuration).SetEase(Ease.Linear).OnComplete(() =>
                {
                    _animator.ResetTrigger(_Run);
                    _animator.SetTrigger(_Idle);
                    
                    endHandle.SetActive(true);
                    _playerHandle.SetActive(false);
                    
                    transform.DOMove(dropPoint.position, jumpDuration).OnComplete(() =>
                    {
                        landEffect.Play();
                        PlayerState.Instance.SetState(PlayerState.State.Default);
                        CamManager.Instance.EnableStartCam();
                        CamManager.Instance.ResetFollowCamTrackedObjectOffset();
                        PlayerState.Instance.DisableMoving();
                    });
                });
            });
        }
    }
}
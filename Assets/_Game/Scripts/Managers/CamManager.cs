
using UnityEngine;
using Cinemachine; // Make sure to import the Cinemachine namespace
using _Game.Managers;
using _Tools.Helpers;
using System;
using DG.Tweening;

namespace _Game
{
    public class CamManager : Singleton<CamManager>
    {

        public static event Action OnCamReseting;
        [Header (" Settings ")]
        public CinemachineVirtualCamera startCam;
        public CinemachineVirtualCamera followCam;
        public CinemachineVirtualCamera endCam;
        public CinemachineVirtualCamera actionCam;

        private CinemachineComposer _followCamComposer;
        private Vector3 _followCamInitialTrackedObjectOffset;
        
        private int startCamPriority = 2;
        private int followCamPriority = 1;
        private int actionCamPriority = 1;
        private float followCamInitialFov;
        private bool enemiesAreDead = false;

        private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
        [Header(" Cam Shake Settings ")]
        [SerializeField][Min(0f)] private float _shakeIntensity = 6f;
        [SerializeField][Min(0f)] private float _shakeDuration = 0.2f;
        [SerializeField] private float _speedBoostShakeIntensity = 0.3f;


        private float _initialCameraShakeIntensity;
        private float _targetCameraShakeDuration;
        private float _currentCameraShakeDuration;
        private bool _canShake;
        private void Start()
        {
            _cinemachineBasicMultiChannelPerlin = actionCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            // Set initial priorities
            startCam.Priority = startCamPriority;
            followCam.Priority = followCamPriority;
            actionCam.Priority = actionCamPriority;

            _followCamComposer = followCam.GetCinemachineComponent<CinemachineComposer>();
            _followCamInitialTrackedObjectOffset = _followCamComposer.m_TrackedObjectOffset;
            followCamInitialFov = actionCam.m_Lens.FieldOfView;

            EnemyUICount.OnAllEnimiesDead += OnAllEnemiesDead;
            // Subscribe to the event
            LineFollower.OnCharacterStartMoving += OnCharacterStartMoving;
            LineFollower.OnCharacterReachDestination += OnDestinationReachedCllBack;
            Enemy.OnDeathResetCam += OnDestinationReachedCllBackDelayed;
            GameManager.Instance.OnEolTrigger += OnEndOfLevelTrigger;
            GameManager.Instance.OnLevelComplete += StopCamFollow;
            AnimationController.OnPlayerComboImpact += OnPlayerComboImpact;
            LineFollower.OnCharacterReachedEnemy += ActionCam;
            AnimationController.OnLastHit += OnLastHit_ShakeCamera;
        }

        private void OnDestroy()
        {
            AnimationController.OnLastHit -= OnLastHit_ShakeCamera;
            LineFollower.OnCharacterReachedEnemy -= ActionCam;
            AnimationController.OnPlayerComboImpact -= OnPlayerComboImpact;
            GameManager.Instance.OnLevelComplete -= StopCamFollow;
            GameManager.Instance.OnEolTrigger -= OnEndOfLevelTrigger;
            Enemy.OnDeathResetCam -= OnDestinationReachedCllBackDelayed;
            LineFollower.OnCharacterStartMoving -= OnCharacterStartMoving;
            LineFollower.OnCharacterReachDestination -= OnDestinationReachedCllBack;
            EnemyUICount.OnAllEnimiesDead -= OnAllEnemiesDead;
        }
        private void OnAllEnemiesDead()
        {
            Invoke("ResetRoutineStartCam", 3);
            enemiesAreDead = true; // Set the flag
        }
        private void ResetRoutineStartCam()
        {
            startCam.Priority = 4;
        }
        private void OnCharacterStartMoving()
        {
            followCam.Priority = startCamPriority + 1;
        }
        void ActionCam(Enemy enemy)
        {
            actionCam.Priority = startCamPriority + 2;
        }
        private void OnDestinationReachedCllBack()
        {
            ResetRoutine();
        }
        private void OnDestinationReachedCllBackDelayed()
        {
            Invoke("ResetRoutine", 3f);
        }

        private void ResetRoutine()
        {
            OnCamReseting?.Invoke();
            if (!enemiesAreDead) // Check the flag before changing camera priority
            {
                startCam.Priority = startCamPriority;
                followCam.Priority = followCamPriority;
                actionCam.Priority = actionCamPriority;
            }
        }
        private void OnEndOfLevelTrigger()
        {
            // Update camera priorities when the event is triggered
            endCam.Priority = 5;
        }

        public void EnableStartCam()
        {
            actionCam.Priority = actionCamPriority;
            startCam.Priority = startCamPriority;
            followCam.Priority = followCamPriority;
        }

        void StopCamFollow(float obj)
        {
          //endCam.Follow = null;
          
        }

        public void SetFollowCamTrackedObjectOffset(Vector3 offset)
        {
            _followCamComposer.m_TrackedObjectOffset = offset;
        }
        
        public void ResetFollowCamTrackedObjectOffset()
        {
            _followCamComposer.m_TrackedObjectOffset = _followCamInitialTrackedObjectOffset;
        }
        private void ZoomFollowCam(float targetFov, float duration)
        {
            // Use DoTween to smoothly change the fov
            DOTween.To(() => actionCam.m_Lens.FieldOfView, fov => actionCam.m_Lens.FieldOfView = fov, targetFov, duration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    // Restore the initial fov after the zoom effect
                    DOTween.To(() => actionCam.m_Lens.FieldOfView, fov => actionCam.m_Lens.FieldOfView = fov, followCamInitialFov, duration)
                        .SetEase(Ease.OutQuad);
                });
        }
        private void OnPlayerComboImpact(object sender, EventArgs e)
        {
            // Trigger the zoom in effect
            ZoomFollowCam(45f, 0.5f);
        }
        void OnLastHit_ShakeCamera() => SetShakeParameters();
        private void SetShakeParameters(float? intensity = null, float? duration = null)
        {
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity ?? _shakeIntensity;
            _initialCameraShakeIntensity = _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain;
            _targetCameraShakeDuration = duration ?? _shakeDuration;
            _currentCameraShakeDuration = _targetCameraShakeDuration;
            _canShake = true;

            Invoke("ResetShakeParameters", _shakeDuration);
            Invoke("esetShakeParametersOnGameEnd", _shakeDuration);
        }
        private void ResetShakeParameters()
        {
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;


        }
        private void ResetShakeParametersOnGameEnd()
        {
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;


        }
    }
}


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

        private CinemachineComposer _followCamComposer;
        private Vector3 _followCamInitialTrackedObjectOffset;
        
        private int startCamPriority = 2;
        private int followCamPriority = 1;
        private float followCamInitialFov;
        private bool enemiesAreDead = false;

        private void Start()
        {
            // Set initial priorities
            startCam.Priority = startCamPriority;
            followCam.Priority = followCamPriority;

            _followCamComposer = followCam.GetCinemachineComponent<CinemachineComposer>();
            _followCamInitialTrackedObjectOffset = _followCamComposer.m_TrackedObjectOffset;
            followCamInitialFov = followCam.m_Lens.FieldOfView;

            EnemyUICount.OnAllEnimiesDead += OnAllEnemiesDead;
            // Subscribe to the event
            LineFollower.OnCharacterStartMoving += OnCharacterStartMoving;
            LineFollower.OnCharacterReachDestination += OnDestinationReachedCllBack;
            Enemy.OnDeathResetCam += OnDestinationReachedCllBackDelayed;
            GameManager.Instance.OnEolTrigger += OnEndOfLevelTrigger;
            GameManager.Instance.OnLevelComplete += StopCamFollow;
            AnimationController.OnPlayerComboImpact += OnPlayerComboImpact;
        }

        private void OnDestroy()
        {
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
            Invoke("ResetRoutineStartCam", 2);
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
        private void OnDestinationReachedCllBack()
        {
            ResetRoutine();
        }
        private void OnDestinationReachedCllBackDelayed()
        {
            Invoke("ResetRoutine", 2f);
        }

        private void ResetRoutine()
        {
            OnCamReseting?.Invoke();
            if (!enemiesAreDead) // Check the flag before changing camera priority
            {
                startCam.Priority = startCamPriority;
                followCam.Priority = followCamPriority;
            }
        }
        private void OnEndOfLevelTrigger()
        {
            // Update camera priorities when the event is triggered
            endCam.Priority = 5;
        }

        public void EnableStartCam()
        {
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
            DOTween.To(() => followCam.m_Lens.FieldOfView, fov => followCam.m_Lens.FieldOfView = fov, targetFov, duration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    // Restore the initial fov after the zoom effect
                    DOTween.To(() => followCam.m_Lens.FieldOfView, fov => followCam.m_Lens.FieldOfView = fov, followCamInitialFov, duration)
                        .SetEase(Ease.OutQuad);
                });
        }
        private void OnPlayerComboImpact(object sender, EventArgs e)
        {
            // Trigger the zoom in effect
            ZoomFollowCam(45f, 0.5f);
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Make sure to import the Cinemachine namespace
using _Game.Managers;
using _Tools.Helpers;

namespace _Game
{
    public class CamManager : Singleton<CamManager>
    {
        [Header (" Settings ")]
        public CinemachineVirtualCamera startCam;
        public CinemachineVirtualCamera followCam;
        public CinemachineVirtualCamera endCam;

        private CinemachineComposer _followCamComposer;
        private Vector3 _followCamInitialTrackedObjectOffset;
        
        private int startCamPriority = 2;
        private int followCamPriority = 1;

        private bool enemiesAreDead = false;

        private void Start()
        {
            // Set initial priorities
            startCam.Priority = startCamPriority;
            followCam.Priority = followCamPriority;

            _followCamComposer = followCam.GetCinemachineComponent<CinemachineComposer>();
            _followCamInitialTrackedObjectOffset = _followCamComposer.m_TrackedObjectOffset;
            
            EnemyUICount.OnAllEnimiesDead += OnAllEnemiesDead;
            // Subscribe to the event
            LineFollower.OnCharacterStartMoving += OnCharacterStartMoving;
            LineFollower.OnCharacterReachDestination += OnDestinationReachedCllBack;
            Enemy.OnDeathResetCam += OnDestinationReachedCllBack;
            GameManager.Instance.OnEolTrigger += OnEndOfLevelTrigger;
            GameManager.Instance.OnLevelComplete += StopCamFollow;

        }

        private void OnDestroy()
        {
            GameManager.Instance.OnLevelComplete -= StopCamFollow;
            GameManager.Instance.OnEolTrigger -= OnEndOfLevelTrigger;
            Enemy.OnDeathResetCam -= OnDestinationReachedCllBack;
            LineFollower.OnCharacterStartMoving -= OnCharacterStartMoving;
            LineFollower.OnCharacterReachDestination -= OnDestinationReachedCllBack;
            EnemyUICount.OnAllEnimiesDead -= OnAllEnemiesDead;
        }
        private void OnAllEnemiesDead()
        {
            startCam.Priority = 4;
            enemiesAreDead = true; // Set the flag
        }

        private void OnCharacterStartMoving()
        {
            followCam.Priority = startCamPriority + 1;
        }
        private void OnDestinationReachedCllBack()
        {
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

        void StopCamFollow()
        {
            endCam.Follow = null;
        }

        public void SetFollowCamTrackedObjectOffset(Vector3 offset)
        {
            _followCamComposer.m_TrackedObjectOffset = offset;
        }
        
        public void ResetFollowCamTrackedObjectOffset()
        {
            _followCamComposer.m_TrackedObjectOffset = _followCamInitialTrackedObjectOffset;
        }
    }
}

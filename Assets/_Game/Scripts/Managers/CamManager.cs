using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Make sure to import the Cinemachine namespace

namespace _Game
{
    public class CamManager : MonoBehaviour
    {
        [Header (" Settings ")]
        public CinemachineVirtualCamera startCam;
        public CinemachineVirtualCamera followCam;

        private int startCamPriority = 2;
        private int followCamPriority = 1;

        private void Start()
        {
            // Set initial priorities
            startCam.Priority = startCamPriority;
            followCam.Priority = followCamPriority;

            // Subscribe to the event
            LineFollower.OnCharacterStartMoving += OnCharacterStartMoving;
            LineFollower.OnCharacterReachDestination += OnDestinationReachedCllBack;
            Enemy.OnDeathResetCam += OnDestinationReachedCllBack;
        }

        private void OnDestroy()
        {
            Enemy.OnDeathResetCam -= OnDestinationReachedCllBack;
            LineFollower.OnCharacterStartMoving -= OnCharacterStartMoving;
            LineFollower.OnCharacterReachDestination -= OnDestinationReachedCllBack;  

        }

        private void OnCharacterStartMoving()
        {
            
            followCam.Priority = startCamPriority + 1;
        }
        private void OnDestinationReachedCllBack()
        {
            startCam.Priority = 2;
            followCam.Priority = 1;
        }
    }
}

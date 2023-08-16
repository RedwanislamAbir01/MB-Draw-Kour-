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
        }

        private void OnDestroy()
        {
            // Unsubscribe from the event to avoid memory leaks
            LineFollower.OnCharacterStartMoving -= OnCharacterStartMoving;
        }

        private void OnCharacterStartMoving()
        {
            // Change follow cam priority when the event is fired
            followCam.Priority = startCamPriority + 1;
        }
    }
}

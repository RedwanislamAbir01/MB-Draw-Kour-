using System;
using _Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public class Highlighter : MonoBehaviour
    {
        [Header ( "Settings " ) ]
        public float rotationSpeed = 30f;

        private Transform firstChild; // Reference to the first child transform

        // Start is called before the first frame update
        void Start()
        {
           
            // Find the first child of this GameObject
            if (transform.childCount > 0)
            {
                firstChild = transform.GetChild(0);
            }

            // Subscribe to events
            GameManager.Instance.OnEolTrigger += DisableObj;
            // Enemy.OnDeathResetCam += OnCharacterReachDestinationHandler;
            LineFollower.OnCharacterStartMoving += OnCharacterStartMovingHandler;
            LineFollower.OnCharacterReachDestination += OnCharacterReachDestinationHandler;
            LineFollower.OnCharacterReachedEnemy += DisableObject;
            CamManager.OnCamReseting += OnCharacterReachDestinationHandler;
            PlayerState.Instance.OnMovementComplete += PlayerState_OnMovementComplete;
        }

        private void OnDisable()
        {
            CamManager.OnCamReseting -= OnCharacterReachDestinationHandler;
            LineFollower.OnCharacterReachedEnemy -= DisableObject;
            GameManager.Instance.OnEolTrigger -= DisableObj;
            // Enemy.OnDeathResetCam -= OnCharacterReachDestinationHandler;
            LineFollower.OnCharacterStartMoving -= OnCharacterStartMovingHandler;
            LineFollower.OnCharacterReachDestination -= OnCharacterReachDestinationHandler;
            PlayerState.Instance.OnMovementComplete -= PlayerState_OnMovementComplete;
        }
        private void FixedUpdate()
        {
           
           
                // Rotate the first child around its up axis
               transform.Rotate(new Vector3(0 , 1, 0) * rotationSpeed * Time.deltaTime);
            
        }

        private void OnCharacterStartMovingHandler()
        {
            // Deactivate the first child
            if (firstChild != null)
            {
                firstChild.gameObject.SetActive(false);
            }
        }
        void DisableObj() => gameObject.SetActive(false);
        private void OnCharacterReachDestinationHandler()
        {
            // Activate the first child
            if (firstChild != null)
            {
                firstChild.gameObject.SetActive(true);
            }
        }
        
        private void PlayerState_OnMovementComplete(object sender, EventArgs e)
        {
            if (firstChild != null)
            {
                firstChild.gameObject.SetActive(true);
            }
        }

        // Unsubscribe from events when the script is disabled or destroyed


        void DisableObject(Enemy enemy)
        {

            if (firstChild != null)
            {
                firstChild.gameObject.SetActive(false);
            }
        }
}
}

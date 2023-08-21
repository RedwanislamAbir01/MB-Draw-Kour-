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
            Enemy.OnDeathResetCam += OnCharacterReachDestinationHandler;
            LineFollower.OnCharacterStartMoving += OnCharacterStartMovingHandler;
            LineFollower.OnCharacterReachDestination += OnCharacterReachDestinationHandler;
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

        private void OnCharacterReachDestinationHandler()
        {
            // Activate the first child
            if (firstChild != null)
            {
                firstChild.gameObject.SetActive(true);
            }
        }

        // Unsubscribe from events when the script is disabled or destroyed
        private void OnDisable()
        {
            Enemy.OnDeathResetCam -= OnCharacterReachDestinationHandler;
            LineFollower.OnCharacterStartMoving -= OnCharacterStartMovingHandler;
            LineFollower.OnCharacterReachDestination -= OnCharacterReachDestinationHandler;
        }
    }
}

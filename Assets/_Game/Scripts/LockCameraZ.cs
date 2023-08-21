using Cinemachine;
using UnityEngine;

namespace _Game
{
    public class LockCameraZ : MonoBehaviour
    {
        public Vector3 offset = new Vector3(0f, 2f, 0f); // Adjust the offset as needed.
        public Transform target; // Assign the target GameObject in the Inspector.

        private CinemachineVirtualCamera vcam;

        private void Start()
        {
            vcam = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            if (target != null)
            {
                // Get the target object's position.
                Vector3 targetPosition = target.position + offset;

                // Set the camera's position to the target position, but keep the X and Y coordinates constant.
                vcam.transform.position = new Vector3(vcam.transform.position.x, vcam.transform.position.y, targetPosition.z);
            }
        }
    }
}

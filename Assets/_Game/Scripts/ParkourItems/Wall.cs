using UnityEngine;

namespace _Game
{
    public class Wall : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.TryGetComponent(out PlayerWallRun playerWallRun))
            {
                playerWallRun.WallRun();
            }
        }
    }
}

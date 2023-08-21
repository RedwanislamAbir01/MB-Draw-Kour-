using UnityEngine;

namespace _Game
{
    public class WoodenPlank : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerPlankWalk playerPlankWalk))
            {
                playerPlankWalk.StartPlankWalk();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerPlankWalk playerPlankWalk))
            {
                playerPlankWalk.StopPlankWalk();
            }
        }
    }
}
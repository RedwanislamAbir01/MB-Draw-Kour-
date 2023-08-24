using UnityEngine;

namespace _Game
{
    public class PlayerItemCollector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectible collectible))
            {
                PlayerInventory.AddItem(collectible.Type, collectible.Value);
                collectible.Collect(transform);
            }
        }
    }
}

using System;
using UnityEngine;

namespace _Game
{
    public class PlayerItemCollector : MonoBehaviour
    {
        public static event EventHandler OnItemCollected;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectible collectible))
            {
                PlayerInventory.AddItem(collectible.Type, collectible.Value);
                collectible.Collect(transform);
                OnItemCollected?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

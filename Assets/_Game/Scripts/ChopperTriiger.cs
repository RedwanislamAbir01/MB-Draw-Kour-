
using System;
using UnityEngine;

namespace _Game
{
    public class ChopperTriiger : MonoBehaviour
    {
        public static event Action OnChopperTrigger;
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                OnChopperTrigger?.Invoke();
            }
        }
    }
}

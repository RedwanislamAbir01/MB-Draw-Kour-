using _Game.Managers;
using System;

using UnityEngine;

namespace _Game
{
    public class PlayerDetector : MonoBehaviour
    {
        public event Action OnPlayerDetected;
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                transform.parent.LookAt(other.transform.position);
                GameManager.Instance.LevelFail();
                OnPlayerDetected?.Invoke();
            }
        }
    }
}

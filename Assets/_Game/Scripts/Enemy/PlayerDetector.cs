using _Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
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
                GameManager.Instance.LevelFail();
                OnPlayerDetected?.Invoke();
            }
        }
    }
}

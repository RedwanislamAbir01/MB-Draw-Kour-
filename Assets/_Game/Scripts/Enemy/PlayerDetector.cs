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

                MeshRenderer childRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
                if (childRenderer != null)
                {
                    Material material = childRenderer.material;
                    Color newColor = childRenderer.material.color = Color.red;
                    newColor.a = 0.50f; // Set the alpha component to 34% (0.34) * always avoid magic numbers like this 
                    material.color = newColor;
                }


            }
        }
    }
}

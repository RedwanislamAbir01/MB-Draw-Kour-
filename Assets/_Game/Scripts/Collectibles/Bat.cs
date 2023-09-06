using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public class Bat : MonoBehaviour
    {
        [SerializeField] private Transform _visual;
        [SerializeField] private Vector3 _rotationAxis = Vector3.forward;
        [SerializeField][Range(1f, 5000f)] private float _rotationSpeed = 500f;

        // Fixed X-axis rotation value
        private float fixedXRotation = -65f;

        private void Update()
        {
            // Calculate the new rotation
            Vector3 newRotation = new Vector3(fixedXRotation, _visual.eulerAngles.y + (_rotationSpeed * Time.deltaTime), 0f);

            // Apply the new rotation
            _visual.rotation = Quaternion.Euler(newRotation);
        }

        public void Collect()
        {
            gameObject.SetActive(false);
            transform.DOKill();
        }
    }
}

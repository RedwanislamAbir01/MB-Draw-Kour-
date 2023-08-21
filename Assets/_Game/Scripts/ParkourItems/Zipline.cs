using UnityEngine;

namespace _Game
{
    public class Zipline : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private Transform _dropPoint;
        [SerializeField] private float _duration = 2f;
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.TryGetComponent(out PlayerZipLining playerZipLining))
            {
                playerZipLining.ZipLine(_startPoint, _endPoint, _dropPoint, _duration);
            }
        }
    }
}

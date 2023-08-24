using UnityEngine;

namespace _Game
{
    public class Zipline : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private Transform _dropPoint;
        [SerializeField] private float _jumpDuration = 1f;
        [SerializeField] private float _zipDuration = 2f;
        [SerializeField] private GameObject _startHandle;
        [SerializeField] private GameObject _endHandle;

        private void Awake()
        {
            _endHandle.SetActive(false);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.TryGetComponent(out PlayerZipLining playerZipLining))
            {
                playerZipLining.ZipLine(_startPoint, _endPoint, _dropPoint, _jumpDuration, _zipDuration, _startHandle, _endHandle);
            }
        }
    }
}

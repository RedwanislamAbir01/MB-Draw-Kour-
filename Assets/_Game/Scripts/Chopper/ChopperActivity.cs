using _Game.Managers;
using DG.Tweening;
using UnityEngine;

namespace _Game
{
    public class ChopperActivity : MonoBehaviour
    {
        public static event System.Action OnChopperReachedDestination;

        [SerializeField] private GameObject _chopperStationPoint;
        [SerializeField]
        private GameObject _visuals;
        [SerializeField] private Transform _standPoint;
        private Vector3 initialPosition;

        Animator _animator; 
        public Transform StandPoint { get => _standPoint; set => _standPoint = value; }

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            StandPoint = transform.GetChild(01);
            _visuals = transform.GetChild(0).gameObject;
            initialPosition = transform.position; // Store the initial position of the chopper
            DisableChopper();
            ChopperTriiger.OnChopperTrigger += GotoStation;
            GameManager.Instance.OnLevelComplete += GoForward;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnLevelComplete -= GoForward;
            ChopperTriiger.OnChopperTrigger -= GotoStation;
        }

        private void GotoStation()
        {
            EnableChopper();

            DOVirtual.DelayedCall(0.0f, () => {
                transform.DOLocalMove(_chopperStationPoint.transform.position, 1f).OnComplete(() => {
                    OnChopperReachedDestination?.Invoke();
                });
            });
        }

        private void GoForward(float obj)
        {
            Invoke("CloseDoor", .1f);
            float forwardDistance = -1000.0f; // You can adjust this distance as needed
            Vector3 targetPosition = initialPosition + new Vector3(forwardDistance, 0, 0);
            _visuals.transform.DOLocalRotate(new Vector3(10, 0, 0), .2f).SetDelay(1).OnComplete(() =>
            {

            });
            transform.DOMove(targetPosition, 100).SetDelay(1).OnComplete(() =>
            {
                // Do something when the chopper completes its forward movement
            });
        }

        private void CloseDoor()
        {
            _animator.SetTrigger("DoorClose");
        }

        private void DisableChopper() => gameObject.SetActive(false);
        private void EnableChopper() => gameObject.SetActive(true);
    }
}

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
        private Vector3 initialPosition;

        private void Start()
        {
            _visuals = transform.GetChild(0).gameObject;
            initialPosition = transform.position; // Store the initial position of the chopper
            DisableChopper();
            GameManager.Instance.OnEolTrigger += GotoStation;
            GameManager.Instance.OnLevelComplete += GoForward;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnLevelComplete -= GoForward;
            GameManager.Instance.OnEolTrigger -= GotoStation;
        }

        private void GotoStation()
        {
            EnableChopper();

            DOVirtual.DelayedCall(1.0f, () => {
                transform.DOLocalMove(_chopperStationPoint.transform.position, 2).OnComplete(() => {
                    OnChopperReachedDestination?.Invoke();
                });
            });
        }

        private void GoForward()
        {
            // Move the chopper forward in the X-axis using DOTween
            float forwardDistance = - 1000.0f; // You can adjust this distance as needed
            Vector3 targetPosition = initialPosition + new Vector3(forwardDistance, 0, 0);
            _visuals.transform.DOLocalRotate(new Vector3( 10, 0, 0), .2f).SetDelay(1);
            transform.DOMove(targetPosition, 100).SetDelay(1).OnComplete(() => {
                // Do something when the chopper completes its forward movement
            });
        }

        private void DisableChopper() => gameObject.SetActive(false);
        private void EnableChopper() => gameObject.SetActive(true);
    }
}

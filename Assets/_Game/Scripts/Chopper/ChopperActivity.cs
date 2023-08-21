using _Game.Managers;
using DG.Tweening;
using UnityEngine;

namespace _Game
{
    public class ChopperActivity : MonoBehaviour
    {
        public static event System.Action OnChopperReachedDestination;

        [SerializeField] private GameObject _chopperStationPoint;
        void Start()
        {
            DisableChopper();
            GameManager.Instance.OnEolTrigger += GotoStation;
        }
        private void OnDestroy()
        {
           
            GameManager.Instance.OnEolTrigger -= GotoStation;
        }

        void GotoStation()
        {
            EnableChopper();

            // Use DOVirtual.DelayedCall to fire the event with a delay
            DOVirtual.DelayedCall(1.0f, () => {
                transform.DOLocalMove(_chopperStationPoint.transform.position, 2).OnComplete(() => {
                    // Fire the static event when the chopper reaches its destination
                    OnChopperReachedDestination?.Invoke();
                });
            });
        }
        void DisableChopper()=> gameObject.SetActive(false);
        void EnableChopper()=> gameObject.SetActive(true);
    }
}

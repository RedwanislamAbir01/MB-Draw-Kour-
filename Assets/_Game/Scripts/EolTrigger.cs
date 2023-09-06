using _Game.Managers;
using DG.Tweening;
using UnityEngine;

namespace _Game
{
    public class EolTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject _visulas;
        private void Start()
        {
            _visulas.transform.DOScale(0.15f, 1f)
                    .SetLoops(-1, LoopType.Yoyo) // Loops the scaling back and forth indefinitely
                    .SetEase(Ease.InOutQuad); // You can change the easing function if needed
            DisableObj();
            EnemyUICount.OnAllEnimiesDead += EnableObj;
        }
        private void OnDestroy()
        {
            EnemyUICount.OnAllEnimiesDead -= EnableObj;
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                _visulas.SetActive(false);
                GetComponent<Collider>().enabled = false;   
                GameManager.Instance.EolTrigger();
            }
        }


        void EnableObj() => gameObject.SetActive(true);
        void DisableObj() => gameObject.SetActive(false); 
    }


}

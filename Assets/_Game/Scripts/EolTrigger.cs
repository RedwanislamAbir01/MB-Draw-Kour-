using _Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public class EolTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject _visulas;
        private void Start()
        {
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

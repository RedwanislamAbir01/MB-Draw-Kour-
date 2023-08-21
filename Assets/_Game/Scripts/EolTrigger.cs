using _Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public class EolTrigger : MonoBehaviour
    {

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
                GameManager.Instance.EolTrigger();
            }
        }


        void EnableObj() => gameObject.SetActive(true);
        void DisableObj() => gameObject.SetActive(false); 
    }


}

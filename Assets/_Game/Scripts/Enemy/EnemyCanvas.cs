using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public class EnemyCanvas : MonoBehaviour
    {

        [SerializeField] private GameObject _sign;
        [SerializeField] private PlayerDetector playerDetector;
        private void Start()
        {
            _sign.SetActive(false);
           
            playerDetector.OnPlayerDetected += EnableExclematorySign;

        }
  
        private void OnDisable()
        {
            playerDetector.OnPlayerDetected -= EnableExclematorySign;
        }


        void EnableExclematorySign()
        {
            _sign.SetActive(true);
            StartCoroutine(SignDisableRoutine());
        }
        IEnumerator SignDisableRoutine()
        {
            yield return new WaitForSeconds(2);
            _sign.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game
{
    public class EnemyCanvas : MonoBehaviour
    {

        [SerializeField] private GameObject _sign;
        [SerializeField] private PlayerDetector playerDetector;
        
        private Enemy _enemy;

        private void Awake()
        {
            _enemy = GetComponentInParent<Enemy>();
        }
        
        private void Start()
        {
            _sign.SetActive(false);
           
            playerDetector.OnPlayerDetected += EnableExclematorySign;
            LineFollower.OnCharacterReachedEnemy += EnableExclematorySign;
        }
  
        private void OnDisable()
        {
            playerDetector.OnPlayerDetected -= EnableExclematorySign;
            LineFollower.OnCharacterReachedEnemy -= EnableExclematorySign;
        }


        void EnableExclematorySign()
        {
            _sign.SetActive(true);
            StartCoroutine(SignDisableRoutine());
        }
        
        void EnableExclematorySign(Enemy enemy)
        {
            if(_enemy != enemy) return;
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

using UnityEngine;
using TMPro;

using System;
using System.Collections;
using _Game.Managers;

namespace _Game
{
    public class EnemyUICount : MonoBehaviour
    {
        public static event Action OnAllEnimiesDead;

        [SerializeField] private TextMeshProUGUI _enemyCount;

        private int totalEnemies;
        private int enemiesKilled;

        private void Start()
        {
            GameManager.Instance.OnLevelFail += DisableObj;
            GameManager.Instance.OnLevelComplete += DisableObj;
            // Find all Enemy objects in the scene
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            totalEnemies = enemies.Length;

            // Initialize the UI text
            UpdateEnemyCountUI();

            // Subscribe to the OnDeath event of all enemies
            foreach (Enemy enemy in enemies)
            {
                enemy.OnDeath += DelayCount;
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe from the OnDeath event of all enemies
            GameManager.Instance.OnLevelFail -= DisableObj;
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.OnDeath -= DelayCount;
            }
        }

        void DelayCount()
        {
            StartCoroutine(CountDealyRoutine());
        }
        IEnumerator CountDealyRoutine()
        {
           yield return new WaitForSeconds(2f);
           HandleEnemyDeath();
        }
        private void HandleEnemyDeath()
        {
            enemiesKilled++;
            UpdateEnemyCountUI();
            if (enemiesKilled == totalEnemies)
            {
                OnAllEnimiesDead?.Invoke();
            }
        }

        private void UpdateEnemyCountUI()
        {
            _enemyCount.text = $"{enemiesKilled}/{totalEnemies}";
        }

        void DisableObj(float a ) => gameObject.SetActive(false);
    }
}

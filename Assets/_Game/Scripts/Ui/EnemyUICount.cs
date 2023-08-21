using UnityEngine;
using TMPro;

using System;

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
            // Find all Enemy objects in the scene
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            totalEnemies = enemies.Length;

            // Initialize the UI text
            UpdateEnemyCountUI();

            // Subscribe to the OnDeath event of all enemies
            foreach (Enemy enemy in enemies)
            {
                enemy.OnDeath += HandleEnemyDeath;
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe from the OnDeath event of all enemies
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.OnDeath -= HandleEnemyDeath;
            }
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
    }
}

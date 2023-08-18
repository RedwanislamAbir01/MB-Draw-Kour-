using System;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damageAmount);
}

public class Enemy : MonoBehaviour, IDamageable
{
    [Header(" Settings ")]
    public int maxHealth = 100;
   

    public event Action OnDeath;
    public event Action OnHit;

    public static event Action OnDeathResetCam;

    private Collider enemyCollider;
    private int currentHealth;
    private void Start()
    {
        currentHealth = maxHealth;
        enemyCollider = GetComponent<Collider>();
    }


    public void TakeDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
            OnHit?.Invoke();
            Debug.Log($"Enemy took {damageAmount} damage. Current health: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        OnDeath?.Invoke();
        enemyCollider.enabled = false;
        OnDeathResetCam?.Invoke();

    }
}

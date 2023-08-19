using System;
using System.Collections;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damageAmount);
}
public enum EnemyType
{
    Static,
    Patrol
}
public class Enemy : MonoBehaviour, IDamageable
{
    [Header(" Settings ")]
    public int maxHealth = 100;
    public GameObject DetectorCone;
    public EnemyType enemyType;
    public event Action OnDeath;
    public event Action OnHit;

    public static event Action OnDeathResetCam;

    private Collider enemyCollider;
    private int currentHealth;
    private void Start()
    {
        currentHealth = maxHealth;
        enemyCollider = GetComponent<Collider>();
        if (enemyType == EnemyType.Patrol)
        {
            StartCoroutine(PatrolRoutine());
            DetectorCone.SetActive(true);
        }
    }


    public void TakeDamage(int damageAmount)
    {
        if (currentHealth > 0)
        {
            StopPatrolMechanics();
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
        Debug.Log("Enemy morlo !");
        OnDeath?.Invoke();
        enemyCollider.enabled = false;
        OnDeathResetCam?.Invoke();
    
    }

    private void StopPatrolMechanics()
    {
        if (enemyType == EnemyType.Patrol)
        {
            StopAllCoroutines(); // Stop the patrol routine
            DetectorCone.SetActive(false);
        }
    }

    private IEnumerator PatrolRoutine()
    {
        float startYRotation = transform.eulerAngles.y; 
        float rotationAmount = 90.0f; 
        float rotationSpeed = 50f; 

        while (true)
        {
            float targetYRotation = startYRotation + rotationAmount;

            while (Mathf.Abs(transform.eulerAngles.y - targetYRotation) > 0.5f) // Check if the rotation is close enough
            {
                float newYRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetYRotation, Time.deltaTime * rotationSpeed);
                transform.rotation = Quaternion.Euler(0.0f, newYRotation, 0.0f);
                yield return null;
            }

            // Snap the rotation to the exact target angle to avoid accumulation of small errors
            transform.rotation = Quaternion.Euler(0.0f, targetYRotation, 0.0f);

          
            yield return new WaitForSeconds(2.0f);

          
            startYRotation = targetYRotation;

            // Prevent startYRotation from exceeding 360 degrees
            if (startYRotation >= 360.0f)
            {
                startYRotation -= 360.0f;
            }
        }
    }




}

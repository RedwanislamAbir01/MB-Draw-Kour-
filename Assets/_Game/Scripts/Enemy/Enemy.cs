using DG.Tweening;
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
    StaticRotating ,
    Patrol
}
public class Enemy : MonoBehaviour, IDamageable
{
    public event Action OnStartMoving;
    public event Action OnStopMoving;

    [Header(" Settings ")]
    public int maxHealth = 100;
    public GameObject DetectorCone;
    public EnemyType enemyType;
    public event Action OnDeath;
    public event Action OnHit;

    public static event Action OnDeathResetCam;

    private Collider enemyCollider;
    private int currentHealth;
    public Transform[] patrolWaypoints; // Array to hold the patrol waypoints
    public float waypointWaitTime = 2.0f;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;
    private void Start()
    {
        DetectorCone.SetActive(true);
        currentHealth = maxHealth;
        enemyCollider = GetComponent<Collider>();
        if (enemyType == EnemyType.StaticRotating)
        {
            StartCoroutine(RotatingEnemyRoutine());

        }
        if (enemyType == EnemyType.Patrol)
        {
            StartCoroutine(PatrolRoutine());

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
        DOTween.Kill(transform);
        StopAllCoroutines(); // Stop the patrol routine
        DetectorCone.SetActive(false);
        
    }

    private IEnumerator RotatingEnemyRoutine()
    {
        float startYRotation = transform.eulerAngles.y;
        float rotationAmount = 90.0f;
        float waitTime = 2.0f;

        while (true) // Infinite loop to keep patrolling
        {
            float targetYRotation = startYRotation + rotationAmount;

            // Use DoTween for rotation
            transform.DORotate(new Vector3(0.0f, targetYRotation, 0.0f), 2, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear);

            yield return new WaitForSeconds(2); // Wait for rotation duration

            startYRotation = targetYRotation;

            if (startYRotation >= 360.0f)
            {
                startYRotation -= 360.0f;
            }

            yield return new WaitForSeconds(waitTime); // Wait for the specified waiting time
        }
    }


    private IEnumerator PatrolRoutine()
    {
        while (true) // Infinite loop to keep patrolling
        {
            if (!isMoving)
            {
                isMoving = true;
                OnStartMoving?.Invoke();
                // Calculate the next waypoint index
                int nextWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;

                // Calculate the direction to the next waypoint
                Vector3 directionToWaypoint = patrolWaypoints[nextWaypointIndex].position - transform.position;
                directionToWaypoint.y = 0.0f; // Keep movement in the horizontal plane
                directionToWaypoint.Normalize();

                // Calculate the target rotation based on the movement direction
                Quaternion targetRotation = Quaternion.LookRotation(directionToWaypoint);

                // Rotate the enemy towards the target waypoint
                transform.DORotateQuaternion(targetRotation, 1.0f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        
                        // Move towards the waypoint
                        transform.DOMove(patrolWaypoints[nextWaypointIndex].position, Vector3.Distance(transform.position, patrolWaypoints[nextWaypointIndex].position))
                            .SetEase(Ease.Linear)
                            .OnComplete(() =>
                            {
                                OnStopMoving?.Invoke();
                                StartCoroutine(WaitAtWaypoint(nextWaypointIndex));
                            });

                        // Update the current waypoint index
                        currentWaypointIndex = nextWaypointIndex;
                    });
            }

            yield return null;
        }
    }

    private IEnumerator WaitAtWaypoint(int waypointIndex)
    {
        yield return new WaitForSeconds(waypointWaitTime);

        // Rotate back to the original rotation
        Quaternion targetRotation = Quaternion.LookRotation(patrolWaypoints[currentWaypointIndex].position - transform.position);
        transform.DORotateQuaternion(targetRotation, 1.0f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                isMoving = false;
             
            });
    }



}

using DG.Tweening;
using System;
using System.Collections;
using _Game;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damageAmount, string punchTriggerName);
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
    public event Action<string> OnHit;


    public static event Action OnDeathResetCam;

    private Collider enemyCollider;
    private int currentHealth;
    public Transform[] patrolWaypoints; // Array to hold the patrol waypoints
    public float waypointWaitTime = 2.0f;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;
    public string punchTriggerName;

    private bool isPatrolling = false;
    private Tweener patrolMoveTween;
    private Tweener patrolRotateTween;
    public bool special;
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

        PlayerHealth.OnDeath += StopPatrolMechanics;
   
    }


    private void OnDestroy()
    {
    
        PlayerHealth.OnDeath -= StopPatrolMechanics;
    }
    public void TakeDamage(int damageAmount, string punchTriggerName)
    {
        if (currentHealth > 0)
        {
            StopPatrolMechanics();
            currentHealth -= damageAmount;
            OnHit?.Invoke(punchTriggerName); // Pass the punch trigger name
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
        StartCoroutine(DeathEffectRoutine());
    }

    private IEnumerator DeathEffectRoutine()
    {
        const float firstDelay = 1f;
        yield return new WaitForSeconds(firstDelay);
        OnDeathResetCam?.Invoke();
        const float secondDelay = 3f;
        yield return new WaitForSeconds(secondDelay);
        PlayerState.Instance.DisableMoving();
    }
    
    private void StopPatrolMechanics()
    {
        isPatrolling = false;
        patrolMoveTween?.Kill();
        patrolRotateTween?.Kill();
        DetectorCone.SetActive(false); StopAllCoroutines();
    }
    public void Stop()
    {
       
        isPatrolling = false;
        patrolMoveTween?.Kill();
        patrolRotateTween?.Kill();
        DetectorCone.SetActive(false); StopAllCoroutines();
    }
    private void StopPatrolMechanics(object sender, EventArgs e)
    {
        isPatrolling = false;
        patrolMoveTween?.Kill();
        patrolRotateTween?.Kill();
        DetectorCone.SetActive(false);
        StopAllCoroutines();
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
            if (!isMoving )
            {
                isMoving = true;
               
                // Calculate the next waypoint index
                int nextWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;

                // Calculate the direction to the next waypoint
                Vector3 directionToWaypoint = patrolWaypoints[nextWaypointIndex].position - transform.position;
                directionToWaypoint.y = 0.0f; // Keep movement in the horizontal plane
                directionToWaypoint.Normalize();

                // Calculate the target rotation based on the movement direction
                Quaternion targetRotation = Quaternion.LookRotation(directionToWaypoint);

                // Rotate the enemy towards the target waypoint
                patrolRotateTween = transform.DORotateQuaternion(targetRotation, 1.0f)
    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        OnStartMoving?.Invoke();
                        // Move towards the waypoint
                        patrolMoveTween = transform.DOMove(patrolWaypoints[nextWaypointIndex].position, Vector3.Distance(transform.position, patrolWaypoints[nextWaypointIndex].position))
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

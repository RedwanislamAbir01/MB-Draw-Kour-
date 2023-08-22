using _Game;
using _Game.Managers;
using System;
using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 

 
 public class LineFollower : MonoBehaviour {
  
    public static event Action<Enemy> OnCharacterReachedEnemy;

    public delegate void CharacterStartMovingDelegate();
    public static event CharacterStartMovingDelegate OnCharacterStartMoving;

    public delegate void CharacterReachDestinationDelegate();
    public static event CharacterReachDestinationDelegate OnCharacterReachDestination;
    /// <summary>
    /// Current main speed 
    /// </summary>
    public float speed;
 
     /// <summary>
     /// The line that this follow.
     /// </summary>
     public LineRenderer lineToFollow;
 
     /// <summary>
     /// So, we have to stop after the first lap?
     /// </summary>
     public bool justOnce = true;
 
     /// <summary>
     /// Internal variable, is the first lap completed?
     /// </summary>
     bool completed = false;
 
     /// <summary>
     /// Follow a smooth path.
     /// </summary>
     public bool smooth = false;
 
     /// <summary>
     /// the number of iterations that split each curve
     /// </summary>
     public int iterations = 10;
     public float radius = 0;
 
     /// <summary>
     /// The points of the line
     /// </summary>
     List<Vector3> wayPoints;
 
     /// <summary>
     /// The Current Point
     /// </summary>
     public int currentPoint = 0;

     public bool endPointReached;
    
    AnimationController controller;
    
     // Use this for initialization
     
     void OnEnable () {

        GameManager.Instance.OnLevelFail += StopMovement;

         Vector3 [] temp = new Vector3[500];
         int total = 0;
         if (lineToFollow != null){
            
             total = lineToFollow.GetPositions(temp);
             wayPoints = new List<Vector3>();
             wayPoints.Clear();
             
             for(int i = 0; i< total; i++)
                 wayPoints.Add(temp[i]);
         }
         completed = false; 
         OnCharacterStartMoving?.Invoke();

     }
    private void OnDestroy()
    {
        GameManager.Instance.OnLevelFail -= StopMovement;
        PlayerHealth.OnDeath -= PlayerHealth_OnDeath; 
    }
    void Start(){
       
        controller = GetComponentInChildren<AnimationController>();
         Vector3 [] temp = new Vector3[500];
         int total = 0;
         if (lineToFollow != null){
             total = lineToFollow.GetPositions(temp);
             wayPoints = new List<Vector3>();
             wayPoints.Clear();
             
             for(int i = 0; i< total; i++)
                 wayPoints.Add(temp[i]);
         }
         completed = false;
         
         PlayerHealth.OnDeath += PlayerHealth_OnDeath; 
     }

    private void PlayerHealth_OnDeath(object sender, EventArgs e)
    {
        ClearLine();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (completed)
        {
            return;
            this.enabled = false;
        }

        if (0 < wayPoints.Count)
        {
            if (smooth)
                FollowSmooth();
            else
            {
                FollowClumsy();
            }
        }
    }


    Vector3 Prevoius(int index){
         if (0 == index) {
             return wayPoints [wayPoints.Count - 1];
         } else {
             return wayPoints [index - 1];
         }
     }
 
  
     Vector3 Current(int index){
         return wayPoints [index];
     }

     Vector3 Next(int index){
         if (wayPoints.Count == index+1) {
             return wayPoints [0];
         } else {
             return wayPoints [index + 1];
         }
     }
 
     void IncreaseIndex(){
      
        currentPoint ++;
         if (currentPoint == wayPoints.Count) {

            endPointReached = true;
            OnCharacterReachDestination?.Invoke();
            OnReachDestination();
             if (justOnce)
                 completed = true;
             else
                 currentPoint = 0;
         }
     }
 
 
 
     void FollowClumsy(){
     
       Vector3 lTargetDir = Current(currentPoint) - transform.position;
        lTargetDir.y = 0.0f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * 0.04f);
        transform.position = Vector3.MoveTowards (transform.position, Current (currentPoint), speed*Time.deltaTime);

         if ((transform.position-Current (currentPoint)).sqrMagnitude < (speed*Time.deltaTime) * (speed*Time.deltaTime) ) {
             IncreaseIndex ();
         }
     }
         
     int i = 1;
 
 
     //the function try, just try, to apply the quadratic beizer algorithm, but thos is based on number of subdivisions, not by speed, so, 
     //the speed varies, usually on closed trurns so, to minimize it I put the splits dependig of speed, but, still is a problem
 
     void FollowSmooth(){
         Vector3 anchor1 = Vector3.Lerp (Prevoius (currentPoint), Current (currentPoint), .5f);
         Vector3 anchor2 = Vector3.Lerp (Current (currentPoint), Next (currentPoint), .5f);
 
         if (i < iterations) {
             float currentProgress = (1f / (float)iterations) * (float)i;
             transform.LookAt (Vector3.Lerp (anchor1, Current (currentPoint), currentProgress));
            transform.position = Vector3.Lerp (Vector3.Lerp (anchor1, Current (currentPoint), currentProgress), Vector3.Lerp (Current (currentPoint), anchor2, currentProgress), currentProgress);
            
            i++;
         } else {
             i = 1;
             IncreaseIndex ();
             Vector3 absisa = Vector3.Lerp (Vector3.Lerp (anchor1, Current (currentPoint), .5f), Vector3.Lerp (Current (currentPoint), anchor2, .5f), .5f);
             float it = (((anchor1-absisa).magnitude + (anchor2 - absisa).magnitude)/(speed*Time.deltaTime));
             iterations = (int)it;
          }
     }
 
     /// <summary>
     /// you can also split the vertexs of the LineRenderer, and you know how to assign it, with setvertex
     /// </summary>
     /// <returns>The vertex.</returns>
     /// <param name="numSplit">Number split.</param>
 
 
     Vector3[] SplitVertex(int numSplit){
         Vector3[] ret = new Vector3[numSplit*wayPoints.Count];
         for(int oldPoint = 0; oldPoint <wayPoints.Count; oldPoint++) {
             Vector3 anchor1 = Vector3.Lerp (Prevoius (oldPoint), Current (oldPoint), .5f);
             Vector3 anchor2 = Vector3.Lerp (Current (oldPoint), Next (oldPoint), .5f);
 
             for (int j = 1; j < numSplit; j++) {
                 float currentProgress = (1f / (float)iterations) * (float)i;
                 ret[oldPoint*numSplit + j] = Vector3.Lerp (Vector3.Lerp (anchor1, Current (oldPoint), currentProgress), Vector3.Lerp (Current (oldPoint), anchor2, currentProgress), currentProgress);
             }
             IncreaseIndex ();
         }
         return ret;
     }
 
    IEnumerator StopCar(float waitTime)
    {
        while(true)
        {
            yield return new WaitForSeconds(waitTime);
            speed = 0;
        }
    }

    void OnReachDestination()
    {
       
        FindAnyObjectByType<path>().ResetPath();
        UpdateWayPoints();
       
    }
    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            transform.LookAt(other.transform);
            OnCharacterReachedEnemy?.Invoke(enemy); // Pass the enemy as a parameter
            ClearLine();
        }
    }

    public void ClearLine()
    {
        FindAnyObjectByType<path>().ResetPath(); // Clear the line
        UpdateWayPoints(); // Update waypoints and reset other necessary variables
    }
    void UpdateWayPoints()
    {
        Vector3[] temp = new Vector3[500];
        int total = 0;
        if (lineToFollow != null)
        {
            total = lineToFollow.GetPositions(temp);
            wayPoints = new List<Vector3>();
            wayPoints.Clear();
            
            for (int i = 0; i < total; i++)
            {
                wayPoints.Add(temp[i]);
            }
        }
        completed = false;
        currentPoint = 0; // Reset currentPoint to start from the beginning
        endPointReached = false; // Reset end point reached flag
    }
    void StopMovement(float arg) => completed = true;

    public float GetSpeed() => speed;

    public Vector3 LastWayPoint()
    {
        return IsWayPointAvailable() ? wayPoints[^1] : Vector3.zero;
    }
    
    public bool IsWayPointAvailable() => wayPoints.Count > 0;

    public void AddToWayPoint(Vector3 point)
    {
        wayPoints.Add(point);
    }
 }
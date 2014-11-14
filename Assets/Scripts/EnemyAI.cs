using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public float patrolSpeed = 1f;                          // The nav mesh agent's speed when patrolling.
    public float chaseSpeed = 5f;                           // The nav mesh agent's speed when chasing.
    public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
    public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
    public Vector2[] patrolWayPoints;                     // An array of transforms for the patrol route.

    private SpriteNavAgent nav;
    private Transform player;                               // Reference to the player's transform.
    private float chaseTimer;                               // A timer for the chaseWaitTime.
    private float patrolTimer;                              // A timer for the patrolWaitTime.
    private int wayPointIndex;                              // A counter for the way point array.
    
    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<SpriteNavAgent>();
    }
    
    void Update () {
        // If the player is in sight and is alive...
//        if(enemySight.playerInSight && playerHealth.health > 0f)
//            // ... shoot.
//            Shooting();
//        
//        // If the player has been sighted and isn't dead...
//        else if(enemySight.personalLastSighting != lastPlayerSighting.resetPosition && playerHealth.health > 0f)
//            // ... chase.
//            Chasing();
//        
//        // Otherwise...
//        else
            // ... patrol.
            Patrolling();
    }

    void Patrolling () {
        // Set an appropriate speed for the NavMeshAgent.
        nav.speed = patrolSpeed;
        
        // If near the next waypoint or there is no destination...
        if(nav.remainingDistance < nav.stoppingDistance)
        {
            // ... increment the timer.
            patrolTimer += Time.deltaTime;
            
            // If the timer exceeds the wait time...
            if(patrolTimer >= patrolWaitTime)
            {
                // ... increment the wayPointIndex.
                if(wayPointIndex == patrolWayPoints.Length - 1)
                    wayPointIndex = 0;
                else
                    wayPointIndex++;
                
                // Reset the timer.
                patrolTimer = 0;
            }
        }
        else
            // If not near a destination, reset the timer.
            patrolTimer = 0;
        
        // Set the destination to the patrolWayPoint.
        Vector2 v = patrolWayPoints[wayPointIndex];
        Int2 d = new Int2((int)v.x, (int)v.y);
        nav.destination = d;
    }
}

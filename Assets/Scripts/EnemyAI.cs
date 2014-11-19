using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpriteTile;

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
    private Int2[] reconstructedPath;

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

    // asdf

    public void generatePath() {
        Int2 start = new Int2(patrolWayPoints[0]);
        Int2 goal = new Int2(patrolWayPoints[1]);

        Debug.Log("generating path from " + start + " to " + goal);

        var closedSet = new HashSet<Int2>();
        var openSet = new PriorityQueue<float, Int2>();
        var cameFrom = new Dictionary<Int2, Int2>();

        var gScore = new Dictionary<Int2, float>();
        var fScore = new Dictionary<Int2, float>();


        // initialize
        gScore[start] = 0;
        fScore[start] = gScore[start] + heuristicCostEstimate(start, goal);
        openSet.Enqueue(fScore[start], start);
        while (openSet.Count > 0) {
            Int2 current = openSet.Dequeue();
            if (current == goal) {
                // return reconstruct path
                reconstructPath(cameFrom, goal);
                return;
            }

            closedSet.Add(current);
            foreach (Int2 neighbor in neighborNodes(current)) {
                if (closedSet.Contains(neighbor)) {
                    continue;
                }

                float tentativeGScore = gScore[current] + distBetween(current, neighbor);

                if (!openSet.ContainsValue(neighbor) || tentativeGScore < gScore[neighbor]) {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + heuristicCostEstimate(neighbor, goal);
                    if (!openSet.ContainsValue(neighbor)) {
                        openSet.Enqueue(fScore[neighbor], neighbor);
                    }
                }
            }
        }

        Debug.Log("Unable to find path");
    }

    void reconstructPath(Dictionary<Int2, Int2> cameFrom, Int2 current) {
        List<Int2> totalPath = new List<Int2>();
        totalPath.Add(current);
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        reconstructedPath = totalPath.ToArray().Reverse().ToArray();
        Debug.Log("Reconstructed path = " + string.Join(",", reconstructedPath.Select(x => x.ToString()).ToArray()));
    }

//    Int2[] neighborNodes(Int2 node) {
//        var neighbors = new Int2[8];
//        var neighborIdx = 0;
//        for (int i = -1; i < 2; i++) {
//            for (int j = -1; j < 2; j++) {
//                if (i != 0 || j != 0) {
//                    neighbors[neighborIdx] = new Int2(node.x+i, node.y+j);
//                    neighborIdx++;
//                }
//            }
//        }
//        return neighbors;
//    }

    Int2[] neighborNodes(Int2 node) {
        var neighbors = new List<Int2>();
        neighbors.Add(new Int2(node.x-1, node.y));
        neighbors.Add(new Int2(node.x+1, node.y));
        neighbors.Add(new Int2(node.x, node.y-1));
        neighbors.Add(new Int2(node.x, node.y+1));

        neighbors = neighbors.FindAll(delegate(Int2 obj) {
            return !Tile.GetCollider(obj);
        });

//        var neighbors = new Int2[4];
//        neighbors[0] = new Int2(node.x-1, node.y);
//        neighbors[1] = new Int2(node.x+1, node.y);
//        neighbors[2] = new Int2(node.x, node.y-1);
//        neighbors[3] = new Int2(node.x, node.y+1);
        return neighbors.ToArray();
    }
    
    float heuristicCostEstimate(Int2 start, Int2 goal) {
        return Mathf.Abs(start.x - goal.x) + Mathf.Abs(start.y - goal.y);
    }

    float distBetween(Int2 a, Int2 b) {
        return heuristicCostEstimate(a, b);
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
                if(wayPointIndex == reconstructedPath.Length - 1)
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
        Int2 d = reconstructedPath[wayPointIndex];
        nav.destination = d;
    }
}

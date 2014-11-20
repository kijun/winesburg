using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpriteTile;

public class EnemyAI : MonoBehaviour {

    enum EnemyAIState {
        Patrolling,
        Chasing,
        Recovering
    }

    public float patrolSpeed = 1f;                          // The nav mesh agent's speed when patrolling.
    public float chaseSpeed = 5f;                           // The nav mesh agent's speed when chasing.
    public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
    public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
    public Vector2[] patrolWayPoints;                     // An array of transforms for the patrol route.
    public float maxSightDistance = 5;
    public float fieldOfViewAngle = 110; // in degrees

    private SpriteNavAgent nav;
    private Transform player;                               // Reference to the player's transform.
    private float chaseTimer;                               // A timer for the chaseWaitTime.
    private float patrolTimer;                              // A timer for the patrolWaitTime.
    private int wayPointIndex;                              // A counter for the way point array.
    private Int2[] reconstructedPath;
    private EnemyAIState state; 

    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<SpriteNavAgent>();
    }

    void Update () {
        if (state == EnemyAIState.Patrolling) {
            if (playerInSight) {
                state = EnemyAIState.Chasing;
            } else {
                Patrolling();
            }
        } else if (state == EnemyAIState.Chasing) {
            Chasing();
        } else {
            Patrolling();
        }
    }

    bool playerInSight() {
        float tileLength = Tile.GetTileSize().x;
        if (Vector2.Distance(player.position, transform.position) / tileLength < maxSightDistance) {
            if (Mathf.Abs(Vector2.Angle(player.position - transform.position, lineOfSight)) < 0.5 * fieldOfViewAngle) {
                return true;
            }
        }
        return false;
    }

    Vector2 lineOfSight {
        get {
            Vector2 currPos = transform.position;
            Vector2 destPos = Tile.GetWorldPosition(nav.destination);

            return destPos - currPos;
        }
    }

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

    Int2[] neighborNodes(Int2 node) {
        var neighbors = new List<Int2>();
        neighbors.Add(new Int2(node.x-1, node.y));
        neighbors.Add(new Int2(node.x+1, node.y));
        neighbors.Add(new Int2(node.x, node.y-1));
        neighbors.Add(new Int2(node.x, node.y+1));

        neighbors = neighbors.FindAll(delegate(Int2 obj) {
            return !Tile.GetCollider(obj);
        });

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
        if(nav.remainingDistance < nav.stoppingDistance) {

            // ... increment the timer.
            patrolTimer += Time.deltaTime;

            // If the timer exceeds the wait time...
            if(patrolTimer >= patrolWaitTime) {
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
        nav.destination = reconstructedPath[wayPointIndex];
    }

    void Chasing () {
        if (this.GetMapPosition().Distance(player.GetMapPosition()) < 0.3) {
            Inventory.mainInventory.RemoveItem(Enzen.InventoryKey);
        }
    }
}

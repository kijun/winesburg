using UnityEngine;
using System.Collections;
using SpriteTile;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    public int numberOfEnemies=10;

    GameObject[] enemies;

	// Use this for initialization
	void Start () {
        Int2 mapSize = Tile.GetMapSize();
        int width = mapSize.x;
        int height = mapSize.y;

        enemies = new GameObject[numberOfEnemies];
        for (int i = 0; i < enemies.Length; i++) {
            Int2 pos;
            do {
                pos = new Int2(Random.Range(1,25), Random.Range(1,25));
                Debug.Log("Pos = " + pos);
            } while (Tile.GetCollider(pos));

            GameObject enemy = (GameObject) Instantiate(enemyPrefab);
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            enemyAI.SetMapPosition(pos);
            enemies[i] = enemy;
            enemyAI.patrolWayPoints = new []{pos.ToVector2(), new Vector2(1,5)};
            enemyAI.generatePath();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

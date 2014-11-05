using UnityEngine;
using System.Collections;
using SpriteTile;

public class SpriteNavAgent : MonoBehaviour {

    public float speed;
    public Int2 destination;
    public float stoppingDistance = 0.3f;

    public float remainingDistance {
        get {
            return Vector3.Distance(this.worldPos, Tile.GetWorldPosition(destination));
        }
    }

	// Update is called once per frame
	void Update () {
        Vector2 destWorldPos = Tile.GetWorldPosition(destination);
        worldPos = Vector3.MoveTowards(this.worldPos, destWorldPos, speed*Time.deltaTime);
	}

    Vector2 worldPos {
        get {
            return gameObject.transform.position;
        }
        set {
            gameObject.transform.position = value;
        }
    }

    Int2 tilePos {
        get {
            return Tile.GetMapPosition(gameObject.transform.position);
        }
        set {
            gameObject.transform.position = Tile.GetWorldPosition(value);
        }
    }

}

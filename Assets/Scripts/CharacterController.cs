using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SpriteTile;

public class CharacterController : MonoBehaviour {

    public SpriteRenderer spriteRenderer;
    public Camera camera;

    public Sprite[] upSprites;
    public Sprite[] downSprites;
    public Sprite[] leftSprites;
    public Sprite[] rightSprites; 
    public float animationSpeed = 0.25f;
    public float characterSpeed = 0.5f; // 1 sprite per second

    private Direction direction;
    private Direction newDirection;
    private Dictionary<Direction, Sprite[]> dirToSprites;
    private float prevAnimTimestamp;

    public void changeDirection (Direction newDir) {
        newDirection = newDir;
    }

	// Use this for initialization
	void Start () {
        dirToSprites = new Dictionary<Direction, Sprite[]>();
        dirToSprites[Direction.Up] = upSprites;
        dirToSprites[Direction.Down] = downSprites;
        dirToSprites[Direction.Left] = leftSprites;
        dirToSprites[Direction.Right] = rightSprites;
        dirToSprites[Direction.None] = upSprites;
	}
	
	// Update is called once per frame
	void Update () {
//        UpdateSpriteWithDirection(direction);
        // TODO: refactor view
        if (newDirection != direction) {
            spriteRenderer.sprite = dirToSprites[newDirection][0];
            direction = newDirection;
            prevAnimTimestamp = Time.time;
        } else {
            UpdateAnimation();
            UpdatePosition();
        }
	}

    void UpdateAnimation() {
        if (prevAnimTimestamp + animationSpeed < Time.time) {
            Sprite[] spritesForCurAnim = dirToSprites[direction];
            var s0 = spritesForCurAnim[0];
            var s1 = spritesForCurAnim[1];
            if (spriteRenderer.sprite == s0) {
                spriteRenderer.sprite = s1;
            } else {
                spriteRenderer.sprite = s0;
            }
            prevAnimTimestamp = Time.time;
        }
    }

    void UpdatePosition() {
        float currX = gameObject.transform.position.x;
        float currY = gameObject.transform.position.y;
        switch (this.direction) {
            case Direction.Up:
                currY += characterSpeed * Time.fixedDeltaTime;
                break;
            case Direction.Down:
                currY -= characterSpeed * Time.fixedDeltaTime;
                break;
            case Direction.Right:
                currX += characterSpeed * Time.fixedDeltaTime;
                break;
            case Direction.Left:
                currX -= characterSpeed * Time.fixedDeltaTime;
                break;
        }

        Vector2 destWorldPos = new Vector2(currX, currY);

        // do not update if 
        bool blocked = Tile.GetCollider(Tile.GetMapPosition(destWorldPos));
        if (!blocked) {
            gameObject.transform.position = new Vector2(currX, currY);
            float cameraZ = camera.transform.position.z;
            camera.transform.position = new Vector3(currX, currY, cameraZ);
        }
    }
}

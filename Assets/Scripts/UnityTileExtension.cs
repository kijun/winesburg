using UnityEngine;
using System.Collections;
using SpriteTile;

public static class UnityTileExtension {
    public static void SetMapPosition(this MonoBehaviour obj, int xPos, int yPos) {
        SetMapPosition(obj.transform, new Int2(xPos, yPos));
    }

    public static void SetMapPosition(this MonoBehaviour obj, Int2 pos) {
        SetMapPosition(obj.transform, pos);
    }

    public static void SetMapPosition(this Transform transform, int xPos, int yPos) {
        SetMapPosition(transform, new Int2(xPos, yPos));
    }
    
    public static void SetMapPosition(this Transform transform, Int2 pos) {
        transform.position = Tile.GetWorldPosition(pos);
    }

    public static Int2 GetMapPosition(this Transform transform) {
        return Tile.GetMapPosition(transform.position);
    }

    public static Int2 GetMapPosition(this MonoBehaviour obj) {
        return Tile.GetMapPosition(obj.transform.position);
    }

    public static float Distance(this Int2 p1, Int2 p2) {
        return Vector2.Distance(p1.ToVector2(), p2.ToVector2());
    }
}
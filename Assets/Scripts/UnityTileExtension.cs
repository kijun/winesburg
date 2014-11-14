using UnityEngine;
using System.Collections;
using SpriteTile;

public static class UnityTileExtension {
    public static void SetMapPosition(this MonoBehaviour obj, int xPos, int yPos) {
        obj.transform.position = Tile.GetWorldPosition(new Int2(xPos, yPos));
    }

    public static void SetMapPosition(this MonoBehaviour obj, Int2 pos) {
        obj.transform.position = Tile.GetWorldPosition(pos);
    }
}  
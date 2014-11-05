using UnityEngine;
using System.Collections;
using SpriteTile;

public class Launcher : MonoBehaviour {
    public TextAsset myLevel;
    public Camera mainCamera;
    public Int2 startPos;

    // Use this for initialization
    void Start () {
        Tile.SetCamera(mainCamera);
        Tile.LoadLevel(myLevel);
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}

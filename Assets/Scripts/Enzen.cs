using UnityEngine;
using System.Collections;

public class Enzen : MonoBehaviour {

    public static string InventoryKey = "enzen";

    private Transform player;

	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (this.GetMapPosition().Distance(player.GetMapPosition()) < 0.3) {
            Inventory.mainInventory.AddItem(InventoryKey);
            Destroy(gameObject);
        }
	}
}

using UnityEngine;
using System.Collections;

public class GameStatusPrinter : MonoBehaviour {

    GUIText inGameConsole;

	// Use this for initialization
	void Start () {
        inGameConsole = GetComponent<GUIText>();
        inGameConsole.text = "";
        NotificationCenter.defaultCenter.AddObserver(Inventory.UpdateNotificationName, HandleInventoryUpdate); 
	}

    void HandleInventoryUpdate() {
        inGameConsole.text = Inventory.mainInventory.ToString();
    }
}

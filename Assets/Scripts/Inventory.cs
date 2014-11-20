using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Inventory {

    // class property / static properties
    private static Inventory mainInventoryInstance;
    public static string UpdateNotificationName = "InventoryUpdated";

    public static Inventory mainInventory {
        get {
            if (mainInventoryInstance == null) mainInventoryInstance = new Inventory();
            return mainInventoryInstance;
        }
    }

    private Inventory() {}

    // object properties
    private Dictionary<string, int>inventoryData = new Dictionary<string, int>();

    public void AddItem(string key) {
        AddItem(key, 1);
    }

    public void AddItem(string key, int numberOfItems) {
        if (numberOfItems < 1) throw new ArgumentOutOfRangeException("invalid param " + numberOfItems);

        var currItemCnt = NumberOfItems(key);
        inventoryData.Remove(key);
        inventoryData.Add(key, currItemCnt + numberOfItems);

        NotificationCenter.defaultCenter.PostNotification(UpdateNotificationName);
    }

    public void RemoveItem(string key) {
        RemoveItem(key, 1);
    }

    public void RemoveAllItems(string key) {
        RemoveItem(key, NumberOfItems(key));
    }

    public void RemoveItem(string key, int numberOfItems) {
        var currItemCnt = NumberOfItems(key);

        if (numberOfItems < 0) throw new ArgumentOutOfRangeException("invalid param " + numberOfItems);
        currItemCnt = Math.Max(currItemCnt - numberOfItems, 0);

        if (currItemCnt == 0) {
            inventoryData.Remove(key);
        } else {
            inventoryData[key] = currItemCnt;
        }
        NotificationCenter.defaultCenter.PostNotification(UpdateNotificationName);
    }

    public Dictionary<string, int>data () {
        return inventoryData;
    }

    public int NumberOfItems(string key) {
        int currItemCnt = 0;
        inventoryData.TryGetValue(key, out currItemCnt);
        return currItemCnt;
    }
    
    public string ToString() {
        return "Inventory: " + String.Join(",", inventoryData.Select(x => String.Format("{0}={1}", x.Key, x.Value)).ToArray());
    }
}

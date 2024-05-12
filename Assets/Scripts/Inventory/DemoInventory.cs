using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoInventory : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] items;

    public void PickupItem(int id) {
        inventoryManager.AddItem(items[id]);
    }
}

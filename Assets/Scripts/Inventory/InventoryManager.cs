using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//    public delegate void onItemChanged();
//    public onItemChanged OnItemChangedCallback;

// https://www.google.com/search?client=firefox-b-1-d&q=unity+inventory#fpstate=ive&vld=cid:e70e38e0,vid:oJAE6CbsQQA,st:0

public class InventoryManager : MonoBehaviour {
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public bool AddItem(Item newItem) {
        // Check if any slot has the same item with a count less than max.
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && 
                itemInSlot.item == newItem && 
                itemInSlot.count < itemInSlot.item.maxStackSize &&
                itemInSlot.item.isStackable) {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        // Check for an empty slot, then spawn the item in that slot
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null) {
                SpawnNewItem(newItem, slot);
                return true;
            }
        }
        return false;
    }

    public void SpawnNewItem(Item newItem, InventorySlot slot) {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(newItem);
    }
}

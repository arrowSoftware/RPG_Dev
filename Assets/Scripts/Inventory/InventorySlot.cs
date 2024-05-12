using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotTag {
    None, Head, Neck, Shoulder, Chest, Wrist, Gloves, Waist, Legs, Feet,
    MainHand, OffHand, Ranged, Ammo,
    Ring, Trinket, PickaxeTool, AxeTool, SickleTool, KnifeTool, FishingTool
}

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotTag {
    None, Head, Neck, Shoulder, Chest, Wrist, Gloves, Waist, Legs, Feet,
    MainHand, OffHand, Ranged, Ammo,
    Ring, Trinket, PickaxeTool, AxeTool, SickleTool, KnifeTool, FishingTool
}

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem myItem { get; set; }
    public bool empty = true;
    public SlotTag myTag;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(Inventory.carriedItem == null) return;
            if(myTag != SlotTag.None && Inventory.carriedItem.myItem.itemTag != myTag) return;
            SetItem(Inventory.carriedItem);
        }
    }

    public void SetItem(InventoryItem item)
    {
        Inventory.carriedItem = null;

        // Reset old slot
        item.activeSlot.empty = true;
        item.activeSlot.myItem = null;

        // Set current slot
        myItem = item;
        myItem.activeSlot = this;
        myItem.transform.SetParent(transform);
        myItem.canvasGroup.blocksRaycasts = true;
        myItem.activeSlot.empty = false;

        if(myTag != SlotTag.None)
        { Inventory.instance.EquipEquipment(myTag, myItem); }
    }
}

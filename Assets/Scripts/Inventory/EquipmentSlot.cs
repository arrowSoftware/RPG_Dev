using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
    public ItemType slotType;
    public GameObject placeholder;

    public void SetPlaceholder(bool enable) {
        placeholder.SetActive(enable);
    }

    public void OnDrop(PointerEventData eventData) {
        if (transform.childCount == 1) {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (inventoryItem.item.itemType == slotType) {
                inventoryItem.parentAfterDrag = transform;
                inventoryItem.transform.SetAsFirstSibling();
                SetPlaceholder(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    [Header("UI")]
    public Image image;
    public TMP_Text countText;

    [HideInInspector] public Transform parentAfterDrag;
    public Item item;
    [HideInInspector] public int count = 1;
    
    public void InitializeItem(Item newItem) {
        item = newItem;
        image.sprite = newItem.icon;
        RefreshCount();
    }

    public void RefreshCount() {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("Begin Drag");
        // If the previous parent was an equipment slot then turn on the placeholder.
        if (transform.parent.TryGetComponent<EquipmentSlot>(out EquipmentSlot slot)) {
            slot.SetPlaceholder(true);
        }
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData) {
        Debug.Log("Drag");
        transform.position = SoftwareCursor.instance.GetCursorPosition();
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("End Drag");
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        // If the drag ended and the item is going back to an equipment slot, turn off the palceholder
        if (transform.parent.TryGetComponent<EquipmentSlot>(out EquipmentSlot slot)) {
            slot.SetPlaceholder(false);
        }
    }
}

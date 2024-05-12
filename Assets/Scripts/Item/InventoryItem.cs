using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler {
    Image itemIcon;

    public CanvasGroup canvasGroup {get; private set;}
    public Item myItem {get; set;}
    public InventorySlot activeSlot {get; set;}

    void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();
    }

    public void Initialize(Item item, InventorySlot parent) {
        activeSlot = parent;
        activeSlot.myItem = this;
        activeSlot.empty = false;
        myItem = item;
        itemIcon.sprite = item.icon;
    }

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("item Click");
        if (eventData.button == PointerEventData.InputButton.Left) {
            Inventory.instance.SetCarriedItem(this);
        }
    }
}

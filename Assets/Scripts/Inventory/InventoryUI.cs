/*
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;

    public Transform itemsParent;
    public InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.OnItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {
    }

    void UpdateUI() {
        for (int i  = 0; i < slots.Length; i++) {
            if (i < inventory.items.Count) {
                slots[i].AddItem(inventory.items[i]);
            } else {
                slots[i].ClearSlot();
            }
        }
    }
}
*/
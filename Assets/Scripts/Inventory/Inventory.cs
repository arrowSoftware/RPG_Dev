using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=d9oLS5hy0zU&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=8
public class Inventory : MonoBehaviour
{
    public delegate void onItemChanged();
    public onItemChanged OnItemChangedCallback;

    public static Inventory instance;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("More that one instance of Inventory found");
            return;
        }
        instance = this;
    }

    public List<Item> items = new List<Item>();

    public int slots = 20;

    public bool Add(Item item) {
        if (!item.isDefaultItem) {
            if (items.Count >= slots) {
                return false;
            }
            items.Add(item);
            if (OnItemChangedCallback != null) {
                OnItemChangedCallback.Invoke();
            }
        }
        return true;
    }

    public void Remove(Item item) {
        items.Remove(item);
        if (OnItemChangedCallback != null) {
            OnItemChangedCallback.Invoke();
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    new public string name = "New Item";
    public Sprite icon = null;
    public SlotTag itemTag;

    [Header("If the item is equiptable")]
    public GameObject equipmentPrefab;
}
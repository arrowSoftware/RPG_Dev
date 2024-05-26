using Unity.VisualScripting;
using UnityEngine;

public enum ItemType {
    None, Head, Neck, Shoulder, Chest, Wrist, Gloves, Waist, Legs, Feet,
    MainHand, OffHand, Ranged, Ammo,
    Ring, Trinket, PickaxeTool, AxeTool, SickleTool, KnifeTool, FishingTool,
    Item
}

public enum ItemQuality {
    Poor, Common, Uncommon, Rare, Epic, Legendary, Artifact
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    new public string name = "New Item";
    public ItemQuality quality;
    public Sprite icon = null;
    public ItemType itemType;
    public bool isStackable;
    public int maxStackSize = 5;

    [Header("If the item is equiptable")]
    public GameObject equipmentPrefab;
}
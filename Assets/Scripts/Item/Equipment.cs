using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipmentSlot;
    public EquipmentMeshRegion[] coveredMeshRegions;
    public SkinnedMeshRenderer mesh;
    public int armorModifier;
    public int damageModifier;

    public override void Use()
    {
        base.Use();

        // Use item
        EquipmentManager.instance.Equip(this);

        // remove from invenory
        RemoveFromInvecntory();
    }

}

public enum EquipmentSlot {
    Head, 
    Neck, 
    Chest, 
    Waist, 
    Legs, 
    Feet, 
    Wrist, 
    Hands, 
    MainHand, 
    OffHand, 
    Ring1, 
    Ring2, 
    Trinket1, 
    Trinket2
}

public enum EquipmentMeshRegion {
    Legs,
    Arms,
    Torso
} // Corresponds to body blend shapes
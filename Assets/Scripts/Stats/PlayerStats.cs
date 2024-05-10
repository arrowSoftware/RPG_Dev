using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    // Start is called before the first frame update
    void Start()
    {
        //EquipmentManager.instance.onEquipmentChanged += onEquipmentChanged;
    }

    void onEquipmentChanged(Equipment newItem, Equipment oldItem) {
        if (newItem != null) {
            armor.AddModifier(newItem.armorModifier);
            physicalDamage.AddModifier(newItem.damageModifier);
        }

        if (oldItem != null) {
            armor.AddModifier(oldItem.armorModifier);
            physicalDamage.AddModifier(oldItem.damageModifier);
        }
    }

    public override void Die()
    {
        base.Die();
        // kill player
        PlayerManager.instance.KillPlayer();
    }
}

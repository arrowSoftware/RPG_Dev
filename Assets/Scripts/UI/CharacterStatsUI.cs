using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStatsUI : MonoBehaviour
{
    CharacterStats stats;
    // Start is called before the first frame update
    void Start()
    {
        stats = PlayerManager.instance.player.transform.GetComponent<CharacterStats>();
        transform.GetChild(1).GetComponent<TMP_Text>().SetText("Level: " + stats.level.ToString());
        transform.GetChild(1).GetComponent<TMP_Text>().SetText("Health: " + stats.health.GetValue().ToString());
        transform.GetChild(2).GetComponent<TMP_Text>().SetText("Power: " + stats.power.GetValue().ToString());
        transform.GetChild(3).GetComponent<TMP_Text>().SetText("Vitality: " + stats.vitality.GetValue().ToString());
        transform.GetChild(4).GetComponent<TMP_Text>().SetText("Energy: " + stats.energy.GetValue().ToString());
        transform.GetChild(5).GetComponent<TMP_Text>().SetText("Strength: " + stats.strength.GetValue().ToString());
        transform.GetChild(6).GetComponent<TMP_Text>().SetText("Agility: " + stats.agility.GetValue().ToString());
        transform.GetChild(7).GetComponent<TMP_Text>().SetText("Intelligence: " + stats.intelligence.GetValue().ToString());
        transform.GetChild(8).GetComponent<TMP_Text>().SetText("Spirit: " + stats.spirit.GetValue().ToString());
        transform.GetChild(9).GetComponent<TMP_Text>().SetText("PhysicalDamage: " + stats.physicalDamage.GetValue().ToString());
        transform.GetChild(10).GetComponent<TMP_Text>().SetText("SpellDamage: " + stats.spellDamage.GetValue().ToString());
        transform.GetChild(11).GetComponent<TMP_Text>().SetText("Defense: " + stats.defense.GetValue().ToString());
        transform.GetChild(12).GetComponent<TMP_Text>().SetText("Armor: " + stats.armor.GetValue().ToString());
        transform.GetChild(13).GetComponent<TMP_Text>().SetText("Parry: " + stats.parry.GetValue().ToString());
        transform.GetChild(14).GetComponent<TMP_Text>().SetText("Block: " + stats.block.GetValue().ToString());
        transform.GetChild(15).GetComponent<TMP_Text>().SetText("Dodge: " + stats.dodge.GetValue().ToString());
        transform.GetChild(16).GetComponent<TMP_Text>().SetText("Critical Chance: " + stats.criticalChance.GetValue().ToString());
        transform.GetChild(17).GetComponent<TMP_Text>().SetText("Attack Power: " + stats.attackPower.GetValue().ToString());
        transform.GetChild(18).GetComponent<TMP_Text>().SetText("Spell Power: " + stats.spellPower.GetValue().ToString());
        transform.GetChild(19).GetComponent<TMP_Text>().SetText("Hit Rating: " + stats.hitRating.GetValue().ToString());
        transform.GetChild(20).GetComponent<TMP_Text>().SetText("Haste Rating: " + stats.hasteRating.GetValue().ToString());
        transform.GetChild(21).GetComponent<TMP_Text>().SetText("Spell Resistance: " + stats.spellResistence.GetValue().ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public float maxHealth {get; private set;}
    private float maxPower;
    public float movementSpeed;
    [Header("Primary Stats")]
    public Stat health; // Life points.
    public Stat power; // "mana", "rage", "focus", "energy"
    public Stat vitality; // Increases health per point. 10 health per vitality point.
    public Stat energy; // Increases power per point.
    public Stat strength; // Increases physical attack power per point.
    public Stat agility; // Increases dodge, pary, crit chances per point.
    public Stat intelligence; // Increases spell power and spell resistance per point.
    public Stat spirit; // Health per second, power per second.
    public Stat physicalDamage; // Damage done by physical attacks
    public Stat spellDamage; // Damage done by magic attacks.
    [Header("Seconday Stats")]
    public Stat defense; // Decreses chance to be hit my physical attacks.
    public Stat armor; // Decreses damage done by physical attacks.
    public Stat parry; // Change to deflect an attack
    public Stat block; // chance to block an attack.
    public Stat dodge; // Chance to dodge attacks.
    public Stat criticalChance; // Chance to land a critical ability (heal,spell,physical)
    public Stat attackPower; // Damamge done with physical attacks
    public Stat spellPower; // Damage done with magic attacks
    public Stat hitRating; // Chance to hit target. TODO
    public Stat hasteRating; // attack speed. TODO
    public Stat spellResistence; // damage redudction to spells.

    // True is the character cannot mvoe.
    public bool isImmobilized = false;
    public bool isSlowed = false;

    public float currentHealth {get; private set;}
    public float currentPower {get; private set;}
    public int level;
    public bool enemy;
    public bool npc;

    [Header("Floating text")]
    public GameObject floatingTextPrefab;

    public event System.Action<float,float> OnHealthChanged;
    public event System.Action<float,float> OnPowerChanged;

    bool sentInitialValues = false;
    bool immune = false;

    private void Start() {}

    public void Awake() {
        // Calculate health value
        health.AddModifier(health.GetBaseValue() * level);
        health.AddModifier(vitality.GetValue() * 10);
        maxHealth = health.GetValue();
        currentHealth = maxHealth;

        // Calculate power value
        power.AddModifier(power.GetBaseValue() * level);
        power.AddModifier(energy.GetValue() * 10);
        maxPower = power.GetValue();
        currentPower = maxPower; 

        // Damage calculation
        physicalDamage.AddModifier(2 * level);
        // Calculate attack power.
        attackPower.AddModifier(strength.GetValue());
        physicalDamage.AddModifier(attackPower.GetValue());
        // Calculate spell power.
        spellPower.AddModifier(2* level);
        spellPower.AddModifier(intelligence.GetValue());
        spellDamage.AddModifier(spellPower.GetValue());

        // Calculate defenses
        // Defense gives 0.04% dodge, 0.04% parry, 0.04% block, 0.04% reduced chance to be hit, 0.04% reduced chance to be crit
        // Block - for every 20 points of defense block is 1% (only with shield active)
        block.AddModifier(defense.GetValue() * 0.05f);
        // parry - for every 20 points of agility parry is 1%
        parry.AddModifier(defense.GetValue() * 0.05f);
        // armor - for every 1 points of agility armor is 2
        armor.AddModifier(agility.GetValue() * 2);
        // dodge - for every 20 points of agility dodge is 1%
        //       - for every 20 points of defense dodge is 1%
        dodge.AddModifier(agility.GetValue() * 0.05f);
        dodge.AddModifier(defense.GetValue() * 0.05f);
        // crit - for every 20 points of agility crit is 1%
        criticalChance.AddModifier(agility.GetValue() * 0.05f);

        // Calculate spell resistence
        spellResistence.AddModifier(intelligence.GetValue());

    }

    private void Update() {
        if (sentInitialValues == false) {
            if (OnHealthChanged != null) {
                OnHealthChanged(maxHealth, currentHealth);
            }
            if (OnPowerChanged != null) {
                OnPowerChanged(maxPower, currentPower);
            }
            sentInitialValues = true;
        }
    }

    public void SetImmune(bool value) {
        immune = value;
    }

    bool GetCritRoll() {
        bool isCrit = false;
        int critRoll = Random.Range(0, 100);
        if (critRoll <= criticalChance.GetValue()) {
            isCrit = true;
        }
        return isCrit;
    }

    public void TakeDamage(CharacterStats casterStats, float damage, Ability ability) {
        if (!immune) {
            bool crit = false;
            // Telegraphed aoe attacks arent based on caster damage, and are envionment AOE.
            if (casterStats != null) {
                crit = casterStats.GetCritRoll();
                if  (crit) {
                    damage *= 2;
                }
            }

            // TODO apply targets crit reduction roll to damage.
            damage -= armor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
        
            currentHealth -= damage;

            GameManager.instance.SendDamageMessage(casterStats.name, ability, this.name, damage, crit);

            // Instantiate floating text
            if (floatingTextPrefab != null) {
                ShowFloatingText(damage.ToString(), false, crit);
            }

            if (OnHealthChanged != null) {
                OnHealthChanged(maxHealth, currentHealth);
            }
        
            if (currentHealth <= 0) {
                Die();
            }
        }
    }

    public void Heal(CharacterStats casterStats, int amount, Ability ability) {
        if (currentHealth <= maxHealth) {
            bool crit = casterStats.GetCritRoll();
            if  (crit) {
                amount *= 2;
            }
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            GameManager.instance.SendHealingMessage(casterStats.name, ability, this.name, amount, crit);

            if (floatingTextPrefab != null) {
                ShowFloatingText(amount.ToString(), true, crit);
            }

            if (OnHealthChanged != null) {
                OnHealthChanged(maxHealth, currentHealth);
            }
        }

    }

    public virtual void Die() {
        // Die
        // ovverides
        Debug.Log(transform.name + " died");
    }

    public void ResetHealth() {
        currentHealth = maxHealth;

        if (OnHealthChanged != null) {
            OnHealthChanged(maxHealth, currentHealth);
        }
    }

    public void ShowFloatingText(string text, bool heal, bool crit) {
        GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
        floatingText.GetComponent<FloatingText>().SetDetails(text, crit, heal, enemy);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bleed Ability", menuName = "Abilites/Bleed")]
public class Bleed : Ability
{
    CharacterStats casterStats;
    CharacterStats targetStats;
    public StatusEffectData statusEffect;

    public override bool Activate(Transform player, Transform target) {
        casterStats = player.gameObject.GetComponent<CharacterStats>();
        targetStats = target.gameObject.GetComponent<CharacterStats>();

        float distance = Vector3.Distance(player.position, target.position);

        // If within range, attack
        if (distance <= maxRange) {
            // If the caster is friendly and target is enemy or the caster is enemy and the target is friendly
            if ((!casterStats.enemy && targetStats.enemy) || (casterStats.enemy && !targetStats.enemy)) {
                float damage = casterStats.physicalDamage.GetValue();
                var effectable = target.GetComponent<IEffectable>();
                if (effectable != null) {
                    effectable.ApplyEffect(casterStats, statusEffect);
                }
                targetStats.TakeDamage(casterStats, damage, this);
                return true;
            }
        } else {
            GameManager.instance.SetWarning();
        }
        return false;
        // todo https://discussions.unity.com/t/how-can-i-use-coroutines-in-scriptableobject/45402/2
    }
}

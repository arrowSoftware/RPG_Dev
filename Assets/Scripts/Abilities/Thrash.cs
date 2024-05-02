using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Thrash Ability", menuName = "Abilites/Thrash")]
public class Thrash : Ability
{
    CharacterStats casterStats;
    CharacterStats targetStats;
    public StatusEffectData statusEffect;

    public override bool Activate(Transform caster, Transform target) {
        casterStats = caster.gameObject.GetComponent<CharacterStats>();
        targetStats = target.gameObject.GetComponent<CharacterStats>();

        float distance = Vector3.Distance(caster.position, target.position);

        // If within range, attack
        if (distance <= maxRange) {
            if ((!casterStats.enemy && targetStats.enemy) || (casterStats.enemy && !targetStats.enemy)) {
                float damage = casterStats.physicalDamage.GetValue();
                var effectable = target.GetComponent<IEffectable>();
                if (effectable != null && statusEffect != null) {
                    effectable.ApplyEffect(casterStats, statusEffect);
                }
                targetStats.TakeDamage(casterStats, damage, this);
                targetStats.TakeDamage(casterStats, damage, this);
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

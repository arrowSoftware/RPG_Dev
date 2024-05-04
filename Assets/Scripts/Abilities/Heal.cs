using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Ability", menuName = "Abilites/Heal")]
public class Heal : Ability
{
    CharacterStats casterStats;
    CharacterStats targetStats;
    public StatusEffectData statusEffect;

    public override bool Activate(Transform caster, Transform target) {
        casterStats = caster.gameObject.GetComponent<CharacterStats>();
        targetStats = target.gameObject.GetComponent<CharacterStats>();

        // If the target is an enemy then heal self, other wise heal the target.
        if (targetStats.enemy) {
            casterStats.Heal(casterStats, 10, this);
            var effectable = caster.GetComponent<IEffectable>();
            if (effectable != null && statusEffect != null) {
                effectable.ApplyEffect(casterStats, statusEffect);
            }
       } else {
            targetStats.Heal(casterStats, 20, this);
            var effectable = target.GetComponent<IEffectable>();
            if (effectable != null && statusEffect != null) {
                effectable.ApplyEffect(casterStats, statusEffect);
            }
        }

        return true;
        // todo https://discussions.unity.com/t/how-can-i-use-coroutines-in-scriptableobject/45402/2
    }
}

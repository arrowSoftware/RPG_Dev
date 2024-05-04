using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HOT Status Effect", menuName = "Status Effects/HOT")]
public class HOTStatusEffect : StatusEffectData
{
    CharacterStats casterStats;
    CharacterStats targetStats;

    public override void Process(Transform caster, Transform target) {
        casterStats = caster.GetComponent<CharacterStats>();
        targetStats = target.GetComponent<CharacterStats>();
        targetStats.Heal(casterStats, valueOverTimeAmount, null);
    }

    public override void Cleanup(Transform target) {}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DOT Status Effect", menuName = "Status Effects/DOT")]
public class DOTStatusEffect : StatusEffectData
{
    CharacterStats casterStats;
    CharacterStats targetStats;

    public override void Process(Transform caster, Transform target) {
        casterStats = caster.GetComponent<CharacterStats>();
        targetStats = target.GetComponent<CharacterStats>();
        targetStats.TakeDamage(casterStats, valueOverTimeAmount, null);
    }

    public override void Cleanup(Transform target) {}
}

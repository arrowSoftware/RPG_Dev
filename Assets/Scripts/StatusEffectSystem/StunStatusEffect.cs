using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stun Status Effect", menuName = "Status Effects/Stun")]
public class StunStatusEffect : StatusEffectData
{
    CharacterStats casterStats;
    CharacterStats targetStats;

    public override void Process(Transform caster, Transform target) {
        casterStats = caster.GetComponent<CharacterStats>();
        targetStats = target.GetComponent<CharacterStats>();
        // Set the targets movement speed to 0.
        target.gameObject.GetComponent<CharacterStats>().isImmobilized = true;
    }

    public override void Cleanup(Transform target) {
        // Set the targets movement speed to 0.
        target.gameObject.GetComponent<CharacterStats>().isImmobilized = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DOT Status Effect", menuName = "Status Effects/DOT")]
public class DOTStatusEffect : StatusEffectData
{
    public override void Process(Transform target) {
        target.GetComponent<CharacterStats>().TakeDamage(valueOverTimeAmount, null, false);
    }
    public override void Cleanup(Transform target) {
    }
}

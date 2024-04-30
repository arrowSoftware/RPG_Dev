using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HOT Status Effect", menuName = "Status Effects/HOT")]
public class HOTStatusEffect : StatusEffectData
{
    public override void Process(Transform target) {
        target.GetComponent<CharacterStats>().Heal(valueOverTimeAmount);
    }
    public override void Cleanup(Transform target) {
    }
}

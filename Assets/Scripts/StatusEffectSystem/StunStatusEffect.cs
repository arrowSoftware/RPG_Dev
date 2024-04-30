using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stun Status Effect", menuName = "Status Effects/Stun")]
public class StunStatusEffect : StatusEffectData
{
    public override void Process(Transform target) {
        // Set the targets movement speed to 0.
        target.gameObject.GetComponent<CharacterStats>().isImmobilized = true;
    }
    public override void Cleanup(Transform target) {
        // Set the targets movement speed to 0.
        target.gameObject.GetComponent<CharacterStats>().isImmobilized = false;
    }
}

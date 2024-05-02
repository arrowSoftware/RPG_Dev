using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placed AOE Spot Ability", menuName = "Abilites/Placed AOE Spot")]
public class PlacedAOE : Ability
{
    public override bool Activate(Transform caster, Transform target) {
        return true;
    }
}

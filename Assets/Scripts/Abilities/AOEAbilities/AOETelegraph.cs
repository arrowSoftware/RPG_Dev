using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AOE Telegraph Ability", menuName = "Abilites/AOE Telegraph")]
public class AOETelegraph : Ability
{
    public override bool Activate(Transform caster, Transform target) { return true; }
}

using UnityEngine;

// Ability scritable object
public class Ability : ScriptableObject
{
    // Name of the ability, overrides the gameobject name
    public new string name;

    // Time between consecutive activations of this avility.
    public float cooldownTime;

    // How long it takes to activate this ability (cast bar)
    public float castTime;

    // Maximum range this ability will work from.
    public float maxRange;

    // Minumum range thi ability will work from.
    public float minRange;

    // Icon is this ability
    public Sprite icon;

    // Activation function, caster is the entity activating this ability, target
    // is the target entitify to receive the effect of this ability.
    // Returns if the ability was acivated, true for activated, false for not.
    public virtual bool Activate(Transform caster, Transform target) {return false;}
}

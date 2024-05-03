using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=J2CFVjqEHpU

[CreateAssetMenu(menuName = "Status Effect")]
public class StatusEffectData : ScriptableObject
{
    public enum StatusEffectType {
        HealOverTime,
        DamageOverTime,
        Stun,
        Slow,
        Silence,
        Root,
        Cleanse,
        Exhaust,
    }

    public new string name;
    public Sprite icon;
    public StatusEffectType type;
    public int valueOverTimeAmount;
    public float tickSpeed;
    public float lifetime;
    public GameObject EffectParticles;

    public virtual void Process(Transform caster, Transform target) {}
    public virtual void Cleanup(Transform target) {}
}

[System.Serializable]
public class StatusEffectStruct {
    public StatusEffectData statusEffectData;
    public CharacterStats casterStats;
    public GameObject statusEffectUiElement;
    public float currentEffectTime;
    public float nextTickTime;
};

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Ability", menuName = "Abilites/Melee")]
public class Melee : Ability
{
    CharacterStats casterStats;
    CharacterStats targetStats;
    Transform caster;
    public StatusEffectData statusEffect;
    public int swingCount;
    public int targetsCount;
    [Range(1,360)]
    public float viewAngle = 60;

    public override bool Activate(Transform caster, Transform target) {
        this.caster = caster;
        casterStats = caster.gameObject.GetComponent<CharacterStats>();
        targetStats = target.gameObject.GetComponent<CharacterStats>();

        float distance = Vector3.Distance(caster.position, target.position);

        // If we can hit more than 1 target
        if (targetsCount > 1) {
            // Find all targets in field of view/attack
            List<CharacterStats> contacts = CheckForContacts();
            for (int i = 0; i < contacts.Count; i++) {
                if ((!casterStats.enemy && contacts[i].enemy) || (casterStats.enemy && !contacts[i].enemy)) {
                    float damage = casterStats.physicalDamage.GetValue();

                    if (contacts[i].TryGetComponent<IEffectable>(out var effectable)) {
                        effectable.ApplyEffect(casterStats, statusEffect);
                    }

                    for (int j = 0; j < swingCount; j++) {
                        contacts[i].TakeDamage(casterStats, damage, this);
                    }
                }
            }
            return true;
        } else {
            if (distance <= maxRange) {
                Vector3 directionToTarget = (target.position - caster.position).normalized;
                // If within field of view/attack
                if (Vector3.Angle(caster.forward, directionToTarget) < viewAngle) {
                    // If the caster is friendly and target is enemy or the caster is enemy and the target is friendly
                    if ((!casterStats.enemy && targetStats.enemy) || (casterStats.enemy && !targetStats.enemy)) {
                        float damage = casterStats.physicalDamage.GetValue();
                        if (target.TryGetComponent<IEffectable>(out var effectable)) {
                            effectable.ApplyEffect(casterStats, statusEffect);
                        }
                        for (int i = 0; i < swingCount; i++) {
                            targetStats.TakeDamage(casterStats, damage, this);
                        }
                        return true;                
                    }
                } else {
                    if (!casterStats.enemy && !casterStats.npc) {
                        GameManager.instance.SetWarning(GameManager.NotificationWarning.InFrontOfTarget);
                    }
                    return false;
                }
            } else {
                if (!casterStats.enemy && !casterStats.npc) {
                    GameManager.instance.SetWarning(GameManager.NotificationWarning.OutOfRange);
                }
                return false;
            }
        }
        
        return false;
        // todo https://discussions.unity.com/t/how-can-i-use-coroutines-in-scriptableobject/45402/2
    }

    List<CharacterStats> CheckForContacts() {
        CharacterStats stats;
        List<CharacterStats> foundAttackables = new List<CharacterStats>();

        // Get all objects within melee range of the caster.
        Collider[] colliders = Physics.OverlapSphere(caster.position, maxRange);
        foreach (Collider collider in colliders) {
            // Check if object is within field of view/attack
            Vector3 directionToTarget = (collider.transform.position - caster.position).normalized;
            // If within field of view/attack
            if (Vector3.Angle(caster.forward, directionToTarget) < viewAngle) {
                if (collider.TryGetComponent<CharacterStats>(out stats)) {
                    foundAttackables.Add(stats);
                }
            }
        }
        return foundAttackables;
    }
}

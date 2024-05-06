using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Ability", menuName = "Abilites/Ranged")]
public class Ranged : Ability
{
    CharacterStats casterStats;
    CharacterStats targetStats;
    Transform caster;
    Transform target;
    public StatusEffectData statusEffect;

    [Range(1,360)]
    public float viewAngle = 60;
    public GameObject projectilePrefab;
    public float speed = 30.0f;

    [Header("Multi Targets")]
    public bool multiTarget = false;
    public float multiTargetRadius = 4.0f;
    
    [Header("Splash Damage")]
    public bool splash = false;
    public float splashRadius = 4.0f;

    public override bool Activate(Transform caster, Transform target) {
        this.caster = caster;
        this.target = target;
        casterStats = caster.gameObject.GetComponent<CharacterStats>();
        targetStats = target.gameObject.GetComponent<CharacterStats>();

        // Get the current distance to the target.
        float distance = Vector3.Distance(caster.position, target.position);


        // If the caster is in range of the target.
        if (distance <= maxRange) {
            Vector3 directionToTarget = (target.position - caster.position).normalized;
            // If within field of view/attack
            if (Vector3.Angle(caster.forward, directionToTarget) < viewAngle) {
                // If multi-target then find x targets within y range of the target, and send a projectile to each on of them.
                if (multiTarget) {
                    List<CharacterStats> contacts = CheckForContacts();
                    for (int i = 0; i < contacts.Count; i++) {
                        if ((!casterStats.enemy && contacts[i].enemy) || (casterStats.enemy && !contacts[i].enemy)) {
                            float damage = casterStats.spellDamage.GetValue();
                            // Spawn the fireball prefab on the player.
                            GameObject projectile = Instantiate(projectilePrefab, caster.GetComponent<ProjectileSpawnPoint>().Point(), Quaternion.identity);

                            // Move the fireball prefab towards the target.
                            projectile.GetComponent<Projectile>().Spawn(this, statusEffect, (int)damage, caster, contacts[i].transform, speed, splash, splashRadius);
                        }
                    }
                    return true;
                } else {
                    // If the caster is friendly and target is enemy or the caster is enemy and the target is friendly
                    if ((!casterStats.enemy && targetStats.enemy) || (casterStats.enemy && !targetStats.enemy)) {
                        float damage = casterStats.spellDamage.GetValue();

                        // Spawn the fireball prefab on the player.
                        GameObject projectile = Instantiate(projectilePrefab, caster.GetComponent<ProjectileSpawnPoint>().Point(), Quaternion.identity);

                        // Move the fireball prefab towards the target.
                        projectile.GetComponent<Projectile>().Spawn(this, statusEffect, (int)damage, caster, target, speed, splash, splashRadius);
                        return true;                
                    }
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

        return true;
    }

    List<CharacterStats> CheckForContacts() {
        CharacterStats stats;
        List<CharacterStats> foundAttackables = new List<CharacterStats>();

        // Get all objects within melee range of the caster.
        Collider[] colliders = Physics.OverlapSphere(target.position, multiTargetRadius);
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

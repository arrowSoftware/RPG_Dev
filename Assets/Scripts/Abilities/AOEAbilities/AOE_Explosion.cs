using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AOE Explosion Ability", menuName = "Abilites/AOE (Explosion)")]
public class AOE_Explosion : Ability
{
    // https://www.youtube.com/watch?v=ode1-TwzNT0 
    Transform caster;
    Transform target;
    CharacterStats casterStats;
    CharacterStats targetStats;
    public GameObject explosionPrefab;
    public bool heal;

    public override bool Activate(Transform caster, Transform target) {
        this.caster = caster;
        this.target = target;
        casterStats = caster.GetComponent<CharacterStats>();
        targetStats = target.GetComponent<CharacterStats>();

        GameObject explosion = Instantiate(explosionPrefab, caster);
        explosion.transform.position = caster.position + new Vector3(0, 1, 0);

        CheckForContacts();
        Destroy(explosion, 0.5f);
        return true;
    }

    void CheckForContacts() {
        CharacterStats stats;
        Collider[] colliders = Physics.OverlapSphere(caster.position, 4.0f);
        foreach (Collider collider in colliders) {
            if (collider.TryGetComponent<CharacterStats>(out stats)) {
                if (heal) {
                    // If the caster is an enemy only heal enemies in the area
                    if (casterStats.enemy && stats.enemy) {
                        stats.Heal(casterStats, 50, this);
                    }
                    // if the caster is a friendly only heal friendlies in the area.
                    if (!casterStats.enemy && !stats.enemy) {
                        stats.Heal(casterStats, 50, this);
                    }
                } else {
                    // If the caster is an enemy only damage friendlies in the area
                    if (casterStats.enemy && !stats.enemy) {
                        stats.TakeDamage(casterStats, 50, this);
                    }
                    // if the caster is a friendly only damage enemies in the area.
                    if (!casterStats.enemy && stats.enemy) {
                        stats.TakeDamage(casterStats, 50, this);
                    }
                }
            }
        }
    }
}

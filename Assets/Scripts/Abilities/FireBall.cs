using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FireBall Ability", menuName = "Abilites/FireBall")]
public class FireBall : Ability
{
    CharacterStats playerStats;
    CharacterStats targetStats;
    public StatusEffectData statusEffect;
    public GameObject fireballPrefab;

    public override bool Activate(Transform player, Transform target) {
        playerStats = player.gameObject.GetComponent<PlayerStats>();
        targetStats = target.gameObject.GetComponent<EnemyStats>();

        // Spawn the fireball prefab on the player.
        GameObject projectile = Instantiate(fireballPrefab, player.GetComponent<ProjectileSpawnPoint>().Point(), Quaternion.identity);

        // Move the fireball prefab towards the target.
        projectile.GetComponent<Projectile>().Spawn(this, statusEffect, 25, player, target, 10);
        return true;
    }
}

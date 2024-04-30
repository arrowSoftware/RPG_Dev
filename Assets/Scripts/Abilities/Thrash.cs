using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Thrash Ability", menuName = "Abilites/Thrash")]
public class Thrash : Ability
{
    CharacterStats playerStats;
    CharacterStats targetStats;
    bool crit = false;
    public StatusEffectData statusEffect;

    public override bool Activate(Transform player, Transform target) {
        playerStats = player.gameObject.GetComponent<PlayerStats>();
        targetStats = target.gameObject.GetComponent<EnemyStats>();

        float distance = Vector3.Distance(player.position, target.position);

        // If within range, attack
        if (distance <= maxRange) {
            target.gameObject.GetComponent<EnemyController>().Aggro(player);
            float damage = playerStats.physicalDamage.GetValue();
            int critRoll = Random.Range(0, 100);
            if (critRoll <= playerStats.criticalChance.GetValue()) {
                damage *= 2;
                crit = true;                
            }
            var effectable = target.GetComponent<IEffectable>();
            if (effectable != null && statusEffect != null) {
                effectable.ApplyEffect(statusEffect);
            }
            targetStats.TakeDamage(damage, this, crit);
            targetStats.TakeDamage(damage, this, crit);
            targetStats.TakeDamage(damage, this, crit);
            // reset crit
            crit = false;
            return true;
        } else {
            GameManager.instance.SetWarning();
        }
        return false;
        // todo https://discussions.unity.com/t/how-can-i-use-coroutines-in-scriptableobject/45402/2
    }
}

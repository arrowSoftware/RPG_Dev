using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stun Ability", menuName = "Abilites/Stun")]
public class Stun : Ability
{
    CharacterStats playerStats;
    CharacterStats targetStats;
    public StatusEffectData statusEffect;

    public override bool Activate(Transform player, Transform target) {
        playerStats = player.gameObject.GetComponent<PlayerStats>();
        targetStats = target.gameObject.GetComponent<EnemyStats>();

        float distance = Vector3.Distance(player.position, target.position);

        // If within range, attack
        if (distance <= maxRange) {
            target.gameObject.GetComponent<EnemyController>().Aggro(player);
            var effectable = target.GetComponent<IEffectable>();
            if (effectable != null && statusEffect != null) {
                effectable.ApplyEffect(statusEffect);
            }
            return true;
        } else {
            GameManager.instance.SetWarning();
        }
        return false;
        // todo https://discussions.unity.com/t/how-can-i-use-coroutines-in-scriptableobject/45402/2
    }
}

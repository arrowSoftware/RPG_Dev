using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Ability", menuName = "Abilites/Heal")]
public class Heal : Ability
{
    CharacterStats playerStats;
    CharacterStats targetStats;
    public StatusEffectData statusEffect;

    public override bool Activate(Transform player, Transform target) {
        playerStats = player.gameObject.GetComponent<PlayerStats>();
        targetStats = target.gameObject.GetComponent<EnemyStats>();

        // If the target is an enemy then heal self, other wise heal the target.
        if (targetStats.enemy) {
            playerStats.Heal(10);
            var effectable = player.GetComponent<IEffectable>();
            effectable.ApplyEffect(statusEffect);
       } else {
            targetStats.Heal(20);
            var effectable = target.GetComponent<IEffectable>();
            effectable.ApplyEffect(statusEffect);
        }

        return true;
        // todo https://discussions.unity.com/t/how-can-i-use-coroutines-in-scriptableobject/45402/2
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    public float attackSpeed = 1.0f;
    private float attackCooldown = 0f;
    public float attackDelay = 0.6f;
    
    public event System.Action OnAttack;

    private CharacterStats myStats;

    AbilityManager abilityManager;
    public List<Ability> abilities = new List<Ability>(10);

    void Start() {
        myStats = GetComponent<CharacterStats>();
        abilityManager = GetComponent<AbilityManager>();
        abilityManager.SetActiveAbilities(abilities);
    }

    void Update() {
        attackCooldown -= Time.deltaTime;
    }

    public void AttackTarget() {}

    public void Attack(Transform target) {
        CharacterStats targetStats = target.GetComponent<CharacterStats>();

        if (attackCooldown <= 0) {
            //abilityManager.HandleAbility(abilities[0]);

            StartCoroutine(DoDamage(targetStats, attackDelay));

            if (OnAttack != null) {
                OnAttack();
            }
            attackCooldown = 1.0f / attackSpeed;
        }
    }

    IEnumerator DoDamage(CharacterStats stats, float delay) {
        yield return new WaitForSeconds(delay);
        stats.TakeDamage(myStats, myStats.physicalDamage.GetValue(), null);
    }
}

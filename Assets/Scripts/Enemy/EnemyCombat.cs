using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatPhase {
    public CombatPhaseCondition condition;
    public List<Ability> phaseAbilities = new List<Ability>();
}

[RequireComponent(typeof(AbilityManager))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(TargetSelection))]
public class EnemyCombat : MonoBehaviour {
    AbilityManager abilityManager;
    Transform target;
    TargetSelection targetSelection;
    CharacterStats myStats;

    public Ability baseAbility;
    public List<Ability> cooldownAbilities = new List<Ability>();
    public List<CombatPhase> phases = new List<CombatPhase>();
    public bool inCombat = false;

    private void Start() {
        abilityManager = GetComponent<AbilityManager>();
        abilityManager.SetActiveAbilities(baseAbility);
        abilityManager.SetActiveAbilities(cooldownAbilities);
        targetSelection = GetComponent<TargetSelection>();
        targetSelection.OnTargetSelected += OnTargetSelected;
        myStats = GetComponent<CharacterStats>();
    }

    void Update() {
        // TODO needs work.
        if (target && inCombat) {
            // loop over all cooldown abilities, if the ability is not on cooldown then activate the ability
            foreach (Ability ability in cooldownAbilities) {
                // If the ability is not on cooldown, then activate it.
                if (abilityManager.IsAbilityOnCooldown(ability) == AbilityManager.CooldownType.noCooldown) {
                    abilityManager.HandleAbility(ability);
                }
            }
            
            // If there are phases then for each phase check its condition, 
            // if the condition is true, loop over the phase abilities and activate those abilities
            foreach (CombatPhase phase in phases) {
                if (phase.condition.CheckCondition(myStats)) {
                    abilityManager.SetActiveAbilities(phase.phaseAbilities);
                    foreach (Ability ability in phase.phaseAbilities) {
                        // If the ability is not on cooldown, then activate it.
                        if (abilityManager.IsAbilityOnCooldown(ability) == AbilityManager.CooldownType.noCooldown) {
                            abilityManager.HandleAbility(ability);
                        }
                    }
                }
            }

            // activate the base ability last. TODO can crete a condition that 
            // is a timed condition, so every 5 seconds activate X ability. 
            // that way the big cooldown arent used on the opener.
            if (abilityManager.IsAbilityOnCooldown(baseAbility) == AbilityManager.CooldownType.noCooldown) {
                abilityManager.HandleAbility(baseAbility);
            }
        }
    }

    public void OnTargetSelected(Transform selection) {
        target = selection;
    }
}

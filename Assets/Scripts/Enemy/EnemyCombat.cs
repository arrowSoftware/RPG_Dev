using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatPhase {
    public CombatPhaseCondition condition;
    public List<Ability> phaseAbilities = new List<Ability>();
}

[RequireComponent(typeof(AbilityManager))]
[RequireComponent(typeof(TargetSelection))]
public class EnemyCombat : MonoBehaviour {
    AbilityManager abilityManager;
    Transform target;
    TargetSelection targetSelection;

    public List<Ability> baseAbilities = new List<Ability>();
    public List<CombatPhase> phases = new List<CombatPhase>();
    public bool inCombat = false;

    private void Start() {
        abilityManager = GetComponent<AbilityManager>();
        abilityManager.SetActiveAbilities(baseAbilities);
        targetSelection = GetComponent<TargetSelection>();
        targetSelection.OnTargetSelected += OnTargetSelected;
    }

    void Update() {
        if (target && inCombat) {
            // loop over all base abilities, if ther ability is not on cooldown then activate the ability
            foreach (Ability ability in baseAbilities) {
                // If the ability is not on cooldown, then activate it.
                if (abilityManager.IsAbilityOnCooldown(ability) == AbilityManager.CooldownType.noCooldown) {
                    abilityManager.HandleAbility(ability);
                }
            }
            
            // If there are phases then for each phase check its condition, if the condition is true, loop over the phase abilities and activate those abilities
        }
    }

    public void OnTargetSelected(Transform selection) {
        target = selection;
    }
}

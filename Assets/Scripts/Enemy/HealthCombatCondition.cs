using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Health Condition Ability", menuName = "ComnatPhaseConditions/Health Condition")]
[System.Serializable]
public class HealthCombatCondition : CombatPhaseCondition
{
    public int healthThreshold;

    public override bool CheckCondition(CharacterStats myStats) {
        this.myStats = myStats;
        if (myStats.currentHealth < healthThreshold) {
            return true;
        }
        return false;
    }
}

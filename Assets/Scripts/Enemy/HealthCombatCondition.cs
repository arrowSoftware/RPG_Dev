using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Health Condition Ability", menuName = "ComnatPhaseConditions/Health Condition")]
[System.Serializable]
public class HealthCombatCondition : CombatPhaseCondition
{
    public int healthThreshold;

    public override bool CheckCondition() {
        return true;
    }
}

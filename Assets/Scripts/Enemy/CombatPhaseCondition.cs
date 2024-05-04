using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatPhaseCondition : ScriptableObject {
    public abstract bool CheckCondition();
}

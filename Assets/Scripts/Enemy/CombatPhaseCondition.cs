using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class CombatPhaseCondition : ScriptableObject {
    public CharacterStats myStats;
    public abstract bool CheckCondition(CharacterStats myStats);
}

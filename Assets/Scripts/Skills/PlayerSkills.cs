using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public List<Skill> skills = new() {
        new(Skill.SkillType.Mining),
        new(Skill.SkillType.Harvesting),
        new(Skill.SkillType.Logging),
        new(Skill.SkillType.Skinning),
        new(Skill.SkillType.Fishing),
    }; 

    public void AddExperience(Skill.SkillType type, int amount) {
        // Find the skill in the skill list
        Skill skill = skills.Find((x) => x.type == type);
        // Add the experience if the skill was found (not null)
        skill?.AddExperience(amount);
    }
}

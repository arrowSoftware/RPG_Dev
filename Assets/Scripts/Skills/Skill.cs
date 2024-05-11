using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public enum SkillType{
        Mining,
        Harvesting,
        Logging,
        Skinning,
        Fishing
    };

    public Skill(SkillType type) {
        this.type = type;
    }

    public SkillType type;

    public int Level {get; private set;} = 1;
    public int MaxLevel {get; private set;} = 99;

    public int Experience {get; private set;} = 0;
    public int MaxExierience {get; private set;} = 200000000;

    public void AddExperience(int amount) {
        // Add the experience 
        Experience += amount;
        Level = LevelAtExperience(Experience);
        GameManager.instance.SendExperienceGainMessage(amount);
    }

    // Return the amount of experience required to reach the next level.
    public int GetExperienceToLevel() {
        return ExperienceForLevel(Level + 1) - Experience;
    }

    // Return the number of expierence required at a certain level.
    int ExperienceForLevel(int level) {
        float total = 0.0f;
        for (int i = 1; i < level; i++) {
            total = Mathf.Floor(i + 300 * Mathf.Pow(2, i / 7.0f));
        }
        return (int)Mathf.Floor(total / 4);
    }

    // Get the current level based on the current experience
    int LevelAtExperience(int experience) {
		float points = 0.0f;
		float output = 0.0f;

		for (int lvl = 1; lvl <= MaxLevel; lvl++) {
			points += Mathf.Floor(lvl + 300 * Mathf.Pow(2, lvl / 7.0f));

			if (lvl >= 1) {
				if (output > experience) {
					lvl--;

					if (lvl == 0) {
						return 1;
					} else if (lvl > 99) {
						return 99;
					} else {
						return lvl;
                    }
				}
				output = Mathf.Floor(points / 4);
			}
		}
		return 0;
    }
}

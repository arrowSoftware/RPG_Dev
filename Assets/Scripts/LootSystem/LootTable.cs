using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An item that can be rolled when a drop is needed.
[System.Serializable]
public class LootSystemObject
{
    public float weight;
    public bool unique;
    public bool always;
    public bool enabled;
    public float[] count = new float[2];

    public Item item;
}

// A loot table.
[System.Serializable]
[CreateAssetMenu(fileName = "New Loot Table", menuName = "Loot Table")]
public class LootTable : ScriptableObject {
    public List<LootSystemObject> contents;
    public float weight;

    public Item GetDrop() {
        float roll = Random.Range(0.0f, 101.0f);
        float weightSum = 0.0f;

        foreach (LootSystemObject loot in contents) {
            weightSum += loot.weight;
            if (roll < weightSum) {
                return loot.item;
            }
        }
        return null;
    }
    
}
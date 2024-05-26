using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableInteractable : Interactable
{
    bool looted = false;
    bool looting = false;
    CastBarUI castbar;

    public GameObject lootable;
    public float respawnTimer;
    public List<LootTable> lootTables;

    private void Start() {
        castbar = CastBarUI.instance;
    }

    public override void Interact() {
        Debug.Log("Interacting with lootable" + name);
        if (!looted) {
            // TODO
            // Player look at the bush
            if (!looting) {
                StartCoroutine(BeginLoot(3.0f)); // Temp 3.0f
            }
        }
    }

    private IEnumerator BeginLoot(float castTime) {
        looting = true;
        if (castbar) {
            castbar.StartCastbar(castTime);
        }
        yield return new WaitForSeconds(castTime);

        // TODO add loot window, then when loot window is closed deactivate the chest.
        // If there are multiple loot tables roll to see which table to loot from.
        float roll = Random.Range(0.0f, 101.0f);
        float weightSum = 0.0f;
        foreach (LootTable loot in lootTables) {
            weightSum += loot.weight;
            if (roll < weightSum) {
                InventoryManager.Instance.AddItem(loot.GetDrop());
                break;
            }
        }
        // Disable the parent object collider, so we dont get more click events.
        transform.GetComponent<Collider>().enabled = false;
        // set the picked boolean.
        looted = true;
        OnDefocused();
        // Disable the node
        lootable.SetActive(false);

        // Start the respawn timer coroutine.
        StartCoroutine(RespawnNode());
    }

    private IEnumerator RespawnNode() {
        yield return new WaitForSeconds(respawnTimer);
        // set the node back to active
        lootable.SetActive(true);
        // Reenable parent collider so click events work
        transform.GetComponent<Collider>().enabled = true;
        looted = false;
        looting = false;
    }
}

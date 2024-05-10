using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningInteractable : Interactable
{
    bool nodeCracked = false;
    bool mining = false;
    CastBarUI castbar;
    
    public GameObject node;
    public float respawnTimer;

    private void Start() {
        castbar = CastBarUI.instance;
    }

    public override void Interact() {
        Debug.Log("Interacting with node" + name);
        if (!nodeCracked) {
            // TODO
            // Player look at the bush
            // Start a cast bar and when the cast bar finishes then perform the below code
            if (!mining) {
                StartCoroutine(BeginMine(3.0f)); // Temp 3.0f
            }
            // When the tree has the force applied award xp and woods
            // If the player moves or hits escape, cancel the cast bar.
            // Cast bar duration is based on tree level, player logging level, axe quality.
            // Awarded xp and woods is based on tree level.
        }
    }

    private IEnumerator BeginMine(float castTime) {
        mining = true;
        if (castbar) {
            castbar.StartCastbar(castTime);
        }
        yield return new WaitForSeconds(castTime);
        // Disable the parent object collider, so we dont get more click events.
        transform.GetComponent<Collider>().enabled = false;
        // set the picked boolean.
        nodeCracked = true;
        // Disable the node
        node.SetActive(false);
        OnDefocused();

        // Start the respawn timer coroutine.
        StartCoroutine(RespawnNode());
    }

    private IEnumerator RespawnNode() {
        yield return new WaitForSeconds(respawnTimer);
        // set the node back to active
        node.SetActive(true);
        // Reenable parent collider so click events work
        transform.GetComponent<Collider>().enabled = true;
        nodeCracked = false;
        mining = false;
    }
}

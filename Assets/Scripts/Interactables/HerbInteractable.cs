using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbInteractable : Interactable
{
    bool herbPicked = false;
    bool picking = false;
    CastBarUI castbar;
    
    public GameObject bush;
    public float respawnTimer;
    public int experienceGainAmount = 1;

    private void Start() {
        castbar = CastBarUI.instance;
    }

    public override void Interact() {
        Debug.Log("Interacting with bush" + name);
        if (!herbPicked) {
            // TODO
            // Player look at the bush
            // Start a cast bar and when the cast bar finishes then perform the below code
            if (!picking) {
                StartCoroutine(BeginPick(3.0f)); // Temp 3.0f
            }
            // When the tree has the force applied award xp and woods
            // If the player moves or hits escape, cancel the cast bar.
            // Cast bar duration is based on tree level, player logging level, axe quality.
            // Awarded xp and woods is based on tree level.
        }
    }

    private IEnumerator BeginPick(float castTime) {
        picking = true;
        if (castbar) {
            castbar.StartCastbar(castTime);
        }
        yield return new WaitForSeconds(castTime);
        // Award experience to the player
        player.GetComponent<PlayerSkills>().AddExperience(Skill.SkillType.Harvesting, experienceGainAmount);
        // Disable the parent object collider, so we dont get more click events.
        transform.GetComponent<Collider>().enabled = false;
        // set the picked boolean.
        herbPicked = true;
        // Disable the bush
        bush.SetActive(false);
        OnDefocused();

        // Start the respawn timer coroutine.
        StartCoroutine(RespawnBush());
    }

    private IEnumerator RespawnBush() {
        yield return new WaitForSeconds(respawnTimer);
        // set the trunk back to active
        bush.SetActive(true);
        // Reenable parent tree collider so click events work
        transform.GetComponent<Collider>().enabled = true;
        herbPicked = false;
        picking = false;
    }
}

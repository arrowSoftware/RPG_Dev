using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TreeInteractable : Interactable
{
    Rigidbody trunkRigidBody;
    bool treeFelled = false;
    readonly float trunkHideTime = 5;
    Vector3 trunkStartPosition;
    Quaternion trunkStartRotation;
    bool chopping = false;

    public Transform forcePoint;
    public GameObject trunk;
    public float respawnTimer;
    public int experienceGainAmount = 1;
    public float chopTime = 3.0f;
    CastBarUI castbar;

    private void Start() {
        trunkStartPosition = trunk.transform.position;
        trunkStartRotation = trunk.transform.rotation;
        castbar = CastBarUI.instance;
    }

    public override void Interact() {
        Debug.Log("Interacting with tree" + name);

        if (!treeFelled) {
            // TODO
            // Player look at the tree
            // Start a cast bar and when the cast bar finishes then perform the below code
            if (!chopping) {
                StartCoroutine(BeginChop(chopTime)); // Temp 3.0f
            }
            // When the tree has the force applied award xp and woods
            // If the player moves or hits escape, cancel the cast bar.
            // Cast bar duration is based on tree level, player logging level, axe quality.
            // Awarded xp and woods is based on tree level.
        }
    }

    private IEnumerator BeginChop(float castTime) {
        chopping = true;
        if (castbar) {
            castbar.StartCastbar(castTime);
        }
        yield return new WaitForSeconds(castTime);
        // Award experience to the player
        player.GetComponent<PlayerSkills>().AddExperience(Skill.SkillType.Logging, experienceGainAmount);
        // Get the rigid body from the trunk
        trunkRigidBody = transform.GetComponentInChildren<Rigidbody>();
        trunkRigidBody.isKinematic = false;
        // Enable the collider of the trunk
        trunk.GetComponent<Collider>().enabled = true;
        // Enable the gravity so it will fall.
        trunkRigidBody.useGravity = true;
        // Apply a random force so it will start falling.
        trunkRigidBody.AddForceAtPosition(new Vector3(2, 0, 2), forcePoint.position); // TODO random direction not 2
        // Disable the parent tree object collider, so we dont get more click events.
        transform.GetComponent<Collider>().enabled = false;
        // set the tree felled boolean.
        treeFelled = true;
        // Start the hide trunk coroutube.
        StartCoroutine(HideTreeTrunk());
        // Start the respawn timer coroutine.
        StartCoroutine(RespawnTree());
    }

    private IEnumerator RespawnTree() {
        yield return new WaitForSeconds(respawnTimer+trunkHideTime);
        // reset the trunks position and rotation
        trunk.transform.SetPositionAndRotation(trunkStartPosition, trunkStartRotation);
        // set the trunk back to active
        trunk.SetActive(true);
        // Reenable parent tree collider so click events work
        transform.GetComponent<Collider>().enabled = true;
        trunkRigidBody.isKinematic = true;
        treeFelled = false;
        chopping = false;
    }

    private IEnumerator HideTreeTrunk() {
        yield return new WaitForSeconds(5);
        // disable the trunk object
        trunk.SetActive(false);
        OnDefocused();
    }
}

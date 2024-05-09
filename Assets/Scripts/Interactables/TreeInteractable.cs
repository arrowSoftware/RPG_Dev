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

    public Transform forcePoint;
    public GameObject trunk;
    public float respawnTimer;

    private void Start() {
        trunkStartPosition = trunk.transform.position;
        trunkStartRotation = trunk.transform.rotation;
    }

    public override void Interact() {
        if (!treeFelled) {
            // TODO
            // Player look at the tree
            // Start a cast bar and when the cast bar finishes then perform the below code
            // When the tree has the force applied award xp and woods
            // If the player moves or hits escape, cancel the cast bar.
            // Cast bar duration is based on tree level, player logging level, axe quality.
            // Awarded xp and woods is based on tree level.

            // Get the rigid body from the trunk
            trunkRigidBody = transform.GetComponentInChildren<Rigidbody>();
            trunkRigidBody.isKinematic = false;
            // Enable the collider of the trunk
            trunk.GetComponent<Collider>().enabled = true;
            // Enable the gravity so it will fall.
            trunkRigidBody.useGravity = true;
            // Apply a random force so it will start falling.
            trunkRigidBody.AddForceAtPosition(new Vector3(Random.Range(-5, 6), 0, Random.Range(-5, 6)), forcePoint.position);
            // Disable the parent tree object collider, so we dont get more click events.
            transform.GetComponent<Collider>().enabled = false;
            // set the tree felled boolean.
            treeFelled = true;
            // Start the hide trunk coroutube.
            StartCoroutine(HideTreeTrunk());
            // Start the respawn timer coroutine.
            StartCoroutine(RespawnTree());
            Debug.Log("Interacting with tree" + name);
        }
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
    }

    private IEnumerator HideTreeTrunk() {
        yield return new WaitForSeconds(5);
        // disable the trunk object
        trunk.SetActive(false);
        OnDefocused();
    }
}

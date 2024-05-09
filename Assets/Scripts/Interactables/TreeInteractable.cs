using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteractable : Interactable
{
    Rigidbody trunkRigidBody;
    public Transform forcePoint;

    private void Start() {
        trunkRigidBody = transform.GetChild(0).GetComponent<Rigidbody>();
    }
    public override void Interact() {
        trunkRigidBody.AddForceAtPosition(new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2)), forcePoint.position);
        Debug.Log("Interacting with tree" + name);
    }
}

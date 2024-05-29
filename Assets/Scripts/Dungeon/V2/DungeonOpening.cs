using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonOpening : MonoBehaviour
{
    public bool isConnected = false;
    public GameObject connectedRoom;

    public void SetConnectedRoom(GameObject room) {
        if (isConnected == false) {
            isConnected = true;
            connectedRoom = room; 
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (isConnected == false && other.TryGetComponent<DungeonOpening>(out DungeonOpening otherOpening)) {
            SetConnectedRoom(other.gameObject);
            otherOpening.SetConnectedRoom(gameObject);
        }
    }

    private void OnDrawGizmos() {
        DrawArrow.ForGizmo(transform.position+new Vector3(0, 2, 0), transform.forward*2, Color.green);
    }
}

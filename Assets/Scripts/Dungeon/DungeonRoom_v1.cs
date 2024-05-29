using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom_v1 : MonoBehaviour
{
    public List<Transform> doorLocations;

    private void OnDrawGizmos() {
        foreach (Transform door in doorLocations) {
            DrawArrow.ForGizmo(door.position+new Vector3(0, 1, 0), door.forward*2, Color.green);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
        Room’s Perspective
        Step 1: Get all the room openings
        Step 2: Iterate over each opening and spawn adjacent chamber
        Step 3: Wait until all adjacent chambers are spawned and are in a valid state
        Step 4: Notify dungeon new rooms are ready
*/
public class DungeonRoom : MonoBehaviour
{
    public List<GameObject> possibleRooms;
    public List<DungeonOpening> openingLocations;
    public bool isValid = true;

    private Transform dungeonParent;
    private IEnumerator coroutine;

    public void Activate(Transform dungeonParent) {
        this.dungeonParent = dungeonParent;
        coroutine = SpawnAdjacentChampers(1.0f);
        StartCoroutine(coroutine);
    }

    public IEnumerator SpawnAdjacentChampers(float waitTime) {
        while (true) {
            yield return new WaitForSeconds(waitTime);
            // Get all the room openings
            // Iterate over each opening and spawn adjacent chamber
            foreach (DungeonOpening opening in openingLocations) {
                // If the opening is not connected, then spawn a chamber.
                if (opening.isConnected == false) {
                    opening.SpawnAdjacentChamper(dungeonParent);
                    break;
                }
            }
            // TODO if all opening are filled, exit this coroutine
        }
    }

    private void OnTriggerEnter(Collider other) {
        // If this collides with another trigger than its not a valid room.
        if (other.isTrigger && !other.gameObject.CompareTag("DungeonOpening")) {
            Debug.Log("COllision " + other.gameObject.tag);
            isValid = false;
        }
    }
}

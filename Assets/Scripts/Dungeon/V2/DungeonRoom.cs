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

    private Transform dungeonParent;

    public void Activate(Transform dungeonParent) {
        this.dungeonParent = dungeonParent;
        Invoke(nameof(SpawnAdjacentChampers), 1f);
    }

    public void SpawnAdjacentChampers() {
        // Get all the room openings
        // Iterate over each opening and spawn adjacent chamber
        foreach (DungeonOpening opening in openingLocations) {
            // If the opening is not connected, then spawn a chamber.
            if (opening.isConnected == false) {
                // Choose a random chamber from the possible list.
                int randomRoom = Random.Range(0, possibleRooms.Count);

                // Spawn the room at the opening point.
                GameObject dungeonRoom = Instantiate(possibleRooms[randomRoom], opening.transform.position, opening.transform.rotation, dungeonParent);
                dungeonRoom.GetComponent<DungeonRoom>().Activate(dungeonParent);
                opening.SetConnectedRoom(dungeonRoom);
                dungeonParent.GetComponent<DungeonGenerator>().AddDungeonRoom(dungeonRoom.GetComponent<DungeonRoom>());
            }
        }
        // Wait until all adjacent chambers are spawned and are in a valid state
        // TODO 
        // Notify dungeon new rooms are ready
    }
}

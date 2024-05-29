using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public List<GameObject> entryRooms;
    
    public List<DungeonRoom> dungeonRooms;
    
    private void Start() {
        Generate();
    }

    /*
        Dungeon’s Perspective
        Step 1: Spawn a room
        Step 2: Get the first available room that has unconnected openings
        Step 3: Tell the room to spawn its’ adjacent chambers
        Step 4: Wait until the room’s openings have spawned
        Step 5: Add Created Rooms to the dungeon
        Step 6: Repeat 2 – 6 until there are no more unconnected openings

        Room’s Perspective
        Step 1: Get all the room openings
        Step 2: Iterate over each opening and spawn adjacent chamber
        Step 3: Wait until all adjacent chambers are spawned and are in a valid state
        Step 4: Notify dungeon new rooms are ready

        Room Opening Level
        Step 1: Get available pieces from the dungeon to spawn
        Step 2: Spawn a connected piece
        Step 3: Wait for the validation period to complete
        Step 4: Mark the opening as connected and notify the 
                    room that a connected chamber is ready

        New Room Validation
        - A room becomes invalid if it intersects another room
        - If it becomes invalid then the room will destroy itself and notify the connection point to try a different piece.
        - If all the pieces are attempted and nothing will fit, then the connection point will close off the room with a wall instead.
    */

    void Generate() {
        // Spawn the random entry room.
        int randomRoom = Random.Range(0, entryRooms.Count);
        GameObject entryRoom = Instantiate(entryRooms[randomRoom], transform.position, Quaternion.identity, transform);
        
        // Set the parent for the new dungeon.
        entryRoom.GetComponent<DungeonRoom>().Activate(transform);

        // Add the entry room.
        AddDungeonRoom(entryRoom.GetComponent<DungeonRoom>());
    }

    public void AddDungeonRoom(DungeonRoom room) {
        dungeonRooms.Add(room);
    }
}

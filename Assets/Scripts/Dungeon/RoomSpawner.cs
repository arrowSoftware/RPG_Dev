using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OpeningDirection {
    North,South,East,West
}

public class RoomSpawner : MonoBehaviour {
    public OpeningDirection openingDirection;
    private RoomTemplates roomTemplates;
    private bool spawned = false;

    private void Start() {
        roomTemplates = GameObject.FindGameObjectWithTag("DungeonRooms").GetComponent<RoomTemplates>();
        Invoke(nameof(Spawn), 2f);
    }

    private void Spawn() {
        if (spawned == false) {
            spawned = true;
            switch (openingDirection) {
                // Needs a room with a south door spawned
                case OpeningDirection.North: {
                    int rand = Random.Range(0, roomTemplates.southRooms.Count);
                    Instantiate(roomTemplates.southRooms[rand], transform.position, roomTemplates.southRooms[rand].transform.rotation);
                    break;
                }
                // Needs a room with a north door spawned
                case OpeningDirection.South: {
                    int rand = Random.Range(0, roomTemplates.northRooms.Count);
                    Instantiate(roomTemplates.northRooms[rand], transform.position, roomTemplates.northRooms[rand].transform.rotation);
                    break;
                }
                // Needs a room with a West door spawned
                case OpeningDirection.East: {
                    int rand = Random.Range(0, roomTemplates.westRooms.Count);
                    Instantiate(roomTemplates.westRooms[rand], transform.position, roomTemplates.westRooms[rand].transform.rotation);
                    break;
                }
                // Needs a room with a East door spawned
            case OpeningDirection.West: {
                    int rand = Random.Range(0, roomTemplates.eastRooms.Count);
                    Instantiate(roomTemplates.eastRooms[rand], transform.position, roomTemplates.eastRooms[rand].transform.rotation);
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("DungeonSpawnPoint")) {
            if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false) {
                // Spawn walls blocking openings
                Instantiate(roomTemplates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }

}

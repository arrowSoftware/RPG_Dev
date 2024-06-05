using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonOpening : MonoBehaviour
{
    public List<GameObject> possibleRooms;

    public bool isConnected = false;
    public GameObject connectedRoom;
    public bool collidedWithOpening = false;
    public GameObject openingDoorPrefab;
    private IEnumerator coroutine = null;
    public bool validOpening = false;
    private Transform dungeonParent;

    [SerializeField]
    private List<int> attemptedRoomIndex;
/*
        Room Opening Level
        Step 1: Get available pieces from the dungeon to spawn
        Step 2: Spawn a connected piece
        Step 3: Wait for the validation period to complete
        Step 4: Mark the opening as connected and notify the 
                    room that a connected chamber is ready
*/

    private void Start() {
        attemptedRoomIndex = new(0);    
    }

    void CloseOpening() {
        Instantiate(openingDoorPrefab, transform.position, transform.rotation, gameObject.transform);
        isConnected = true;
        if (coroutine != null) {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void SpawnAdjacentChamper(Transform dungeonParent) {
        //if (dungeonParent.GetComponent<DungeonGenerator>().IsDungoenFull()) {
        //    CloseOpening();
        //    return;
        //}

        this.dungeonParent = dungeonParent;
        if (isConnected == false) {
            // Choose a random chamber from the possible list.
            int randomRoom = 0;
            for (int i = 0; i < possibleRooms.Count; i++) {
                randomRoom = Random.Range(0, possibleRooms.Count);
                if (attemptedRoomIndex.Contains(randomRoom)) {
                    // Try a room we havent tried yet.
                    continue;
                } else {
                    attemptedRoomIndex.Add(randomRoom);
                }
            }

            // If we have tried all the rooms, close off the opening
            if (attemptedRoomIndex.Count == possibleRooms.Count) {
                CloseOpening();
            } else {
                // if the dungeon is filled we need to close this opening instead.
                if (dungeonParent.GetComponent<DungeonGenerator>().IsDungoenFull()) {
                    CloseOpening();
                } else {
                    // Spawn the room at the opening point.
                    GameObject dungeonRoom = Instantiate(possibleRooms[randomRoom], transform.position, transform.rotation, dungeonParent);

                    // Wait for the validation period to complete.
                    coroutine = Validate(0.25f, dungeonRoom);
                    StartCoroutine(coroutine);
                }
            }
        }
    }

    private IEnumerator Validate(float waitTime, GameObject dungeonRoom)
    {
        yield return new WaitForSeconds(waitTime);
        dungeonRoom.TryGetComponent<DungeonRoom>(out DungeonRoom room);
        if (!room.isValid) {
            Destroy(dungeonRoom);
            isConnected = false;
        } else {
            isConnected = true;
            connectedRoom = dungeonRoom;
            // add room to the dungeon master
            dungeonParent.GetComponent<DungeonGenerator>().AddDungeonRoom(room);
            room.Activate(dungeonParent);
        }
    }

    private void OnTriggerEnter(Collider other) {
        // If this opening collides with another opening, join the two.
        if (other.TryGetComponent<DungeonOpening>(out DungeonOpening opening)) {
            opening.isConnected = true;
            opening.connectedRoom = this.gameObject;
        }    
    }

    private void OnDrawGizmos() {
        DrawArrow.ForGizmo(transform.position+new Vector3(0, 2, 0), transform.forward*2, Color.green);
    }
}

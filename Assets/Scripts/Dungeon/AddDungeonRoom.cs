using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDungeonRoom : MonoBehaviour
{
    private RoomTemplates roomTemplates;

    private void Start() {
        roomTemplates = GameObject.FindGameObjectWithTag("DungeonRooms").GetComponent<RoomTemplates>();
        roomTemplates.rooms.Add(this.gameObject);
    }
}

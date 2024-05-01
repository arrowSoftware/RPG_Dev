using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawnPoint : MonoBehaviour
{
    public Transform point;
    public Vector3 Point() { return point.position; }
}

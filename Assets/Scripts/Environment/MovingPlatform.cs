using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    [SerializeField]
    private WaypointPath waypointPath;

    [SerializeField]
    private float speed;

    private int targetWaypointIndex;

    private Transform previousWaypoint;
    private Transform targetWaypoint;

    private float timeToWaypoint;
    private float timeElapsed;

    private bool waypointReached;

    // Pausing
    public bool enablePausePlatform = true;
    public float pausePlatformTime = 5.0f;
    private bool platformPaused = false;
    private float pauseTimer;

    void Start() {
        pauseTimer = 0f;
        TargetNextWaypoint();
    }

    void FixedUpdate() {
        float elapsedPercentage = 0;
        if (waypointReached && enablePausePlatform) {
            pauseTimer += Time.deltaTime;
            platformPaused = true;
            if (pauseTimer >= pausePlatformTime) {
                pauseTimer = 0;
                waypointReached = false;
                platformPaused = false;
            }
        }

        if ((enablePausePlatform && !platformPaused) || !enablePausePlatform) {
            timeElapsed += Time.deltaTime;

            elapsedPercentage = timeElapsed / timeToWaypoint;
            transform.position = Vector3.Lerp(previousWaypoint.position, targetWaypoint.position, elapsedPercentage);
        }

        if (elapsedPercentage >= 1) {
            waypointReached = true;
            TargetNextWaypoint();
        }
    }

    private void TargetNextWaypoint() {
        previousWaypoint = waypointPath.GetWaypoint(targetWaypointIndex);
        targetWaypointIndex = waypointPath.GetNextWaypointIndex(targetWaypointIndex);
        targetWaypoint = waypointPath.GetWaypoint(targetWaypointIndex);

        timeElapsed = 0;

        float distanceToWaypoint = Vector3.Distance(previousWaypoint.position, targetWaypoint.position);
        timeToWaypoint = distanceToWaypoint / speed;
    }

    private void OnTriggerEnter(Collider other) {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other) {
        other.transform.SetParent(null);
    }
}

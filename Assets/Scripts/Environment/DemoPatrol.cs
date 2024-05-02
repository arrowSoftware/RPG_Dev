using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPatrol : MonoBehaviour
{
    [SerializeField]
    private WaypointPath waypointPath;

    [SerializeField]
    private float speed;

    private int targetWaypointIndex;

    private Transform previousWaypoint;
    private Transform targetWaypoint;

    private float timeToWaypoint;
    private float timeElapsed;

    void Start() {
        TargetNextWaypoint();
    }

    void FixedUpdate() {
        float elapsedPercentage = 0;
        timeElapsed += Time.deltaTime;

        elapsedPercentage = timeElapsed / timeToWaypoint;
        transform.position = Vector3.Lerp(previousWaypoint.position, targetWaypoint.position, elapsedPercentage);

        if (elapsedPercentage >= 1) {
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
}

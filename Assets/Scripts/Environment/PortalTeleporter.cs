using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && other.TryGetComponent<PlayerControls>(out var player)) {
            player.Teleport(destination.position, destination.rotation);
        }    
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(destination.position, 0.4f);
        Vector3 direction = destination.TransformDirection(Vector3.forward);
        Gizmos.DrawRay(destination.position, direction);
    }
}

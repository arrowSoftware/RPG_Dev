using UnityEngine;

// https://www.youtube.com/watch?v=HQNl3Ff2Lpo&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=5
public class Interactable : MonoBehaviour
{
    public float radius = 3.0f; // distance to get to object to interact.
    bool isFocus = false;
    public Transform player;
    bool hasInteracted = false;
    public Transform interactionTransform;

    public virtual void Interact() {
    }

    private void Update() {
        if (isFocus && !hasInteracted) {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= radius) {
                Interact();
                hasInteracted = true;
            }
        }    
    }

    public void OnFocused(Transform playerTransform) {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused() {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    private void OnDrawGizmosSelected() {
        if (interactionTransform == null){
            interactionTransform = transform;
        }
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);        
    }
}
 
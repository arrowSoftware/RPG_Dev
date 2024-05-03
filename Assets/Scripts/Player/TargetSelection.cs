using UnityEngine;
using UnityEngine.EventSystems;

public class TargetSelection : MonoBehaviour
{
    public SoftwareCursor swCursor;

    [SerializeField]
    private Transform selection;

    [SerializeField]
    private Transform highlight;

    public GameObject selectionCirclePrefab;
    private GameObject selectionCircle;
    public Canvas canvas;

    public Interactable focus;
    private RaycastHit rayCastHit;

    public event System.Action<Transform> OnTargetSelected;
    public bool npc = false;
    
    void Update() {
        if (npc) {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        

        if (highlight != null) {
            highlight = null;
        }

        // Cast a ray at the mouse position 
        Ray ray = Camera.main.ScreenPointToRay(swCursor.GetCursorPosition());
        if (Physics.Raycast(ray, out rayCastHit)) {
            // store the transform of the "hover" hit object
            highlight = rayCastHit.transform;
            if (rayCastHit.collider.TryGetComponent<Interactable>(out Interactable interactable)) {
                // Interactable object found
            } else {
                // Not an interactable object, clear the highlight
                highlight = null;
            }
        }

        // Left click select.
        if (swCursor.GetMouseButtonDown(0)) {
            // If there is a highlighted interactable
            if (highlight) {
                // Set the selction
                selection = highlight;
                // Clear the highlight
                highlight = null;
                // Broadcast the new selection.
                if (OnTargetSelected != null) {
                    OnTargetSelected(selection);
                }

                // If there was a selection circle spawned already, destroy it before placing a new one.
                if (selectionCircle != null) {
                    Destroy(selectionCircle);
                }

                // Create the new selection circle.
                selectionCircle = Instantiate(selectionCirclePrefab, canvas.transform);
                selectionCircle.transform.localScale = new Vector3(2, 2);
            }
        }

        // Right mouse button, interact
        if (swCursor.GetMouseButtonDown(1)) {
            if (highlight) {
                selection = rayCastHit.transform;
                highlight = null;

                Interactable interactable = rayCastHit.collider.GetComponent<Interactable>();
                if (interactable != null) {
                    SetFocus(interactable);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (selection) {
                selection = null;
                RemoveFocus();
                if (OnTargetSelected != null) {
                    OnTargetSelected(null);
                }
                Destroy(selectionCircle);
            }
        }
    }

    void LateUpdate()
    {
        if (npc) {
            return;
        }

        // If there is a selection circle then move the selection circle with the target.
        if (selectionCircle != null) {
            selectionCircle.transform.position = selection.position;
            // Attach the selection circle at the feet of the target.
            // set the scale of the circle, relative to the target.
        }
    }

    void SetFocus(Interactable newFocus) {
        if (newFocus != focus) {
            if (focus != null) {
                focus.OnDefocused();
            }
            focus = newFocus;
        }
        newFocus.OnFocused(transform);
    }

    void RemoveFocus() {
        if (focus != null) {
            focus.OnDefocused();            
        }
        focus = null;
    }

    public void SetTarget(Transform target) {
        if (OnTargetSelected != null) {
            OnTargetSelected(target);
        }
    }
}

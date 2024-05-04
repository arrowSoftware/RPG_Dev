using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetSelection : MonoBehaviour
{
    SoftwareCursor swCursor;

    [SerializeField]
    private Transform selection;

    [SerializeField]
    private Transform highlight;

    public Interactable focus;

    public GameObject selectionCirclePrefab;
    public Canvas canvas;
    
    private GameObject selectionCircle;
    private RaycastHit rayCastHit;

    public event System.Action<Transform> OnTargetSelected;

    // true if this script is controlled by an npc
    public bool npc = false;
    
    private void Start() {
        swCursor = SoftwareCursor.instance;
    }

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
            float distance = 20.0f;
            Vector3 hitLocation;
            // Attach the selection circle at the feet of the target.
            if (Physics.Raycast(selection.position, Vector3.down, out RaycastHit hit, distance)) {
                hitLocation = hit.point;
                selectionCircle.transform.position = new Vector3(selection.position.x, hitLocation.y + 0.05f, selection.position.z);
            } else {
                // If the target is already on the ground.
                selectionCircle.transform.position = selection.position + new Vector3(0,  + 0.05f, 0);
            }
            // set the scale of the circle, relative to the target.
            // Get the collider attached to the character, find the largest value and scale the cirlce to it.
            Collider col = selection.GetComponent<Collider>();
            Vector3 colSize = col.bounds.size;
            float scaleFactor = FindLargestValueInVector3(colSize) * 2;
            selectionCircle.transform.localScale = new Vector3(scaleFactor, scaleFactor);
            
            // Set the color based on target friendlyness
            // Get the selections character stats
            CharacterStats stats = selection.GetComponent<CharacterStats>();
            if (stats.enemy) {
                // Set the selecion color to red
                selectionCircle.GetComponent<Image>().color = Color.red;
            } else if (stats.npc) {
                // Set the color to yellow
                selectionCircle.GetComponent<Image>().color = Color.yellow;
            } else {
                // Set the color to green.
                selectionCircle.GetComponent<Image>().color = Color.green;
            }
        }
    }

    float FindLargestValueInVector3(Vector3 vec) {
        float largest = vec.x;
        if (vec.z > largest) {
            largest = vec.z;
        }
        return largest;
        
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

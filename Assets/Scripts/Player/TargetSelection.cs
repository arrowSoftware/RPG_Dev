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

        Ray ray = Camera.main.ScreenPointToRay(swCursor.GetCursorPosition());

        if (Physics.Raycast(ray, out rayCastHit)) {
            highlight = rayCastHit.transform;
            Interactable interactable = rayCastHit.collider.GetComponent<Interactable>();

            if (interactable != null /*&& highlight != selection*/) {

            } else {
                highlight = null;
            }
        }

        // Left click select.
        if (swCursor.GetMouseButtonDown(0)) {
            if (highlight) {
                selection = rayCastHit.transform;
                highlight = null;
                if (OnTargetSelected != null) {
                    OnTargetSelected(selection);
                }

                if (selectionCircle != null) {
                    Destroy(selectionCircle);
                }
                selectionCircle = Instantiate(selectionCirclePrefab, canvas.transform);
                selectionCircle.transform.localScale = new Vector3(2, 2);
            } /*else {
                if (selection) {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    selection = null;
                    OnTargetSelected(null);
                    RemoveFocus();
                }
            }*/
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
            }/* else {
                if (selection) {
                    selection.gameObject.GetComponent<Outline>().enabled = false;
                    selection = null;
                    RemoveFocus();
                }
            }*/
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

        if (selectionCircle != null) {
            selectionCircle.transform.position = selection.position;
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

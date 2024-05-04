using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeChecker : MonoBehaviour
{
    public float nameplateDetectionRange = 40;
    List<Transform> nameplatesInRange = new List<Transform>();
    public bool onlyShowEnemyNameplates = false;

    void LateUpdate() {
        CheckForContacts();
        for (int i = 0; i < nameplatesInRange.Count; i++) {
            // If the nameplate is still in range, keep it shown, else hide it
            float distance = Vector3.Distance(transform.position, nameplatesInRange[i].position);

            if (distance <= nameplateDetectionRange) {
                nameplatesInRange[i].GetComponent<NameplateUI>().ShowNameplate(true);
            } else {
                nameplatesInRange[i].GetComponent<NameplateUI>().ShowNameplate(false);
                nameplatesInRange.Remove(nameplatesInRange[i]);
            }
        }
    }

    void CheckForContacts() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, nameplateDetectionRange);
        foreach (Collider collider in colliders) {
            if (collider.TryGetComponent<NameplateUI>(out NameplateUI ui)) {
                if (onlyShowEnemyNameplates) {
                    if (ui.transform.GetComponent<CharacterStats>().enemy) {
                        if (!nameplatesInRange.Contains(collider.transform)) {
                            nameplatesInRange.Add(collider.transform);
                        }
                    }
                } else {
                    if (!nameplatesInRange.Contains(collider.transform)) {
                        nameplatesInRange.Add(collider.transform);
                    }
                }
            }
        }
    }    

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, nameplateDetectionRange);
    }
}

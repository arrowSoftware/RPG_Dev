using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETelegraphScalar : MonoBehaviour
{
    public Transform innerScalarSection;
    public Transform outerSection;
    public float timeToFill = 10.0f;
    public Vector3 scaleDirection;
    public bool looping = false;
    public float damageValue = 100.0f;
    CharacterStats casterStats;

    bool isScaling = false;
    Coroutine coroutine;
    Vector3 initialScale;
    List<Collider> currentCollisions = new List<Collider>();

    public enum AOESpotShape {
        AOECircle,
        AOEBox,
        AOEPoly
    }
    public AOESpotShape spotShape;

    void Start() {
        initialScale = innerScalarSection.localScale;
        coroutine = StartCoroutine(ScaleOverTime(innerScalarSection, outerSection.localScale, timeToFill));
    }

    void Update() {
        if (looping && isScaling == false) {
            innerScalarSection.localScale = initialScale;
            coroutine = StartCoroutine(ScaleOverTime(innerScalarSection, outerSection.localScale, timeToFill));
        }

        // TODO better boolean for this, not all scalars use the y direction
        if (innerScalarSection.localScale.y >= outerSection.localScale.y) {
            CheckForContacts();
        }
        
    }

    IEnumerator ScaleOverTime(Transform objectToScale, Vector3 targetScale, float duration) {
        //Make sure there is only one instance of this function running
        if (isScaling) {
            yield break;
        }
        isScaling = true;

        float counter = 0;

        //Get the current scale of the object to be moved
        Vector3 startScaleSize = objectToScale.localScale;

        while (counter < duration) {
            counter += Time.deltaTime;
            objectToScale.localScale = Vector3.Lerp(startScaleSize, targetScale, counter / duration);
            yield return null;
        }
        isScaling = false;
    }

    List<Collider> GetCurrentColliders() {
        switch (spotShape) {
            case AOESpotShape.AOECircle: {
                return new List<Collider>(Physics.OverlapSphere(transform.position, transform.GetComponent<SphereCollider>().radius));
            }
            case AOESpotShape.AOEBox: {
                return currentCollisions;
            }
            case AOESpotShape.AOEPoly: {
                return currentCollisions;
            }
        }
        return null;
    }

    void CheckForContacts() {
        List<Collider> colliders = GetCurrentColliders();

        foreach (Collider collider in colliders) {
            if (collider.GetComponent<CharacterStats>()) {
                collider.GetComponent<CharacterStats>().TakeDamage(null, damageValue, null);
            }
        }
        colliders.Clear();
    }

	void OnTriggerEnter(Collider other) {
        currentCollisions.Add(other);
    }

    private void OnTriggerExit(Collider other) {
        currentCollisions.Remove(other);
    }
}

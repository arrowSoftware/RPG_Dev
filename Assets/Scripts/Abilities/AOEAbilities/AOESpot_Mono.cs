using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class AOESpot_Mono : MonoBehaviour
{
    float tickInterval;
    int valuePerTick;
    List<Collider> currentCollisions = new List<Collider>();

    public enum AOESpotShape {
        AOECircle,
        AOEBox,
        AOEPoly
    }
    public AOESpotShape spotShape;

    public enum AOESpotType {
        AOESpotHeal,
        AOESpotDamage,
    }
    public AOESpotType spotType;

    public void SetAOEDetails(float tickInterval, AOESpotType type, int valuePerTick) {
        this.tickInterval = tickInterval;
        spotType = type;
        this.valuePerTick = valuePerTick;
    }

    void LateUpdate() {
        tickInterval += Time.deltaTime;

        if (tickInterval >= 1) {
            tickInterval = 0;
    		CheckForContacts();
        }
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
                switch (spotType) {
                    case AOESpotType.AOESpotHeal: {
                        collider.GetComponent<CharacterStats>().Heal(valuePerTick);
                        break;
                    }
                    case AOESpotType.AOESpotDamage: {
                        collider.GetComponent<CharacterStats>().TakeDamage(valuePerTick, null, false);
                        break;
                    }
                }
            }
        }
    }

	void OnTriggerEnter(Collider other) {
        currentCollisions.Add(other);
    }

    private void OnTriggerExit(Collider other) {
        currentCollisions.Remove(other);
    }
}

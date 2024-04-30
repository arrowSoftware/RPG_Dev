using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESpot_Mono : MonoBehaviour
{
    float tickInterval;
    int valuePerTick;

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

    void CheckForContacts() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 6.0f);
        foreach (Collider collider in colliders) {
            if (collider.GetComponent<CharacterStats>()) {
                switch (spotType) {
                    case AOESpotType.AOESpotHeal: {
                        collider.GetComponent<CharacterStats>().Heal(this.valuePerTick);
                        break;
                    }
                    case AOESpotType.AOESpotDamage: {
                        collider.GetComponent<CharacterStats>().TakeDamage(this.valuePerTick, null, false);
                        break;
                    }
                }
            }
        }
    }
}

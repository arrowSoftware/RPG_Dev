using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.TextCore.Text;

public class AOESpot_Mono : MonoBehaviour
{
    float tickInterval;
    float currentTick;
    int valuePerTick;
    List<Collider> currentCollisions = new List<Collider>();
    CharacterStats casterStats;
    CharacterStats targetStats;
    Transform caster;
    Transform target;
    Ability ability;

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

    public void SetAOEDetails(Transform caster, Transform target, Ability ability, float tickInterval, AOESpotType type, int valuePerTick) {
        this.caster = caster;
        this.target = target;
        this.ability = ability;
        this.tickInterval = tickInterval;
        spotType = type;
        this.valuePerTick = valuePerTick;
        currentTick = 0;
        casterStats = caster.GetComponent<CharacterStats>();
        targetStats = caster.GetComponent<CharacterStats>();
    }

    void Update() {
        // leash the spot to the fround layer so it doenst follow they player when he jumps.
        // Every update frame send a raycast out and update the spots position to be at the hit point + offset.

        float distance = 20.0f;
        Vector3 hitLocation;
        if (Physics.Raycast(caster.position, Vector3.down, out RaycastHit hit, distance)) {
            hitLocation = hit.point;
            transform.position = new Vector3(transform.position.x, hitLocation.y + 0.05f, transform.position.z);
        }
    }

    void LateUpdate() {
        currentTick += Time.deltaTime;

        if (currentTick >= tickInterval) {
            currentTick = 0;
    		CheckForContacts();
        }
    }

    List<Collider> GetCurrentColliders() {
        switch (spotShape) {
            case AOESpotShape.AOECircle: {
                return new List<Collider>(Physics.OverlapSphere(transform.position, transform.GetComponent<SphereCollider>().radius));
            }
            case AOESpotShape.AOEBox:
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
                CharacterStats stats = collider.GetComponent<CharacterStats>();
                switch (spotType) {
                    case AOESpotType.AOESpotHeal: {
                        // If the healing spot was placed by a caster on an enemy target, only heal other friendlies in that zone.
                        // If the healing spot was placed by a caster on a friendly target, heal all friendlies in the zone.
                        // If the healing spot was placed by a caster on the ground, heal all friendlies in the zone.

                        // If the caster is an enemy then apply the heals to all enemies
                        if (casterStats.enemy) {
                            if (stats.enemy) {
                                stats.Heal(casterStats, valuePerTick);
                            }
                        } else {
                            // If the caster is a friendly, apply heals to all friendlies
                            if (!stats.enemy) {
                                stats.Heal(casterStats, valuePerTick);                                
                            }
                        }
                        break;
                    }
                    case AOESpotType.AOESpotDamage: {
                        // If the damage spot was placed by a caster on an enemy target, only damage other enemies in that zone.
                        // If the damage spot was placed by a caster on a friendly target, damage all enemies in the zone.
                        // If the damage spot was placed by a caster on the ground, damage all enemies in the zone.

                        // If the caster is an enemy then apply the damage to all friendlies
                        if (casterStats.enemy) {
                            if (!stats.enemy) {
                                stats.TakeDamage(casterStats, valuePerTick, ability);
                            }
                        } else {
                            // If the caster i a friendly, apply damage to all enemies.
                            if (stats.enemy) {
                                stats.TakeDamage(casterStats, valuePerTick, ability);
                            }
                        }
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

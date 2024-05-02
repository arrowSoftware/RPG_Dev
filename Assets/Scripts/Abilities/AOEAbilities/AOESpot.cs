using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AOE Spot Ability", menuName = "Abilites/AOE Spot")]
public class AOESpot : Ability
{
    Transform caster;
    Transform target;
    public GameObject AOESpotHealPrefab;
    public AOESpot_Mono.AOESpotType aoeSpotType;
    public int valuePerTick;
    public int tickInterval;
    public int duration;

    public override bool Activate(Transform caster, Transform target) {
        this.caster = caster;
        this.target = target;
        GameObject spot;
        if (placedAOE) {
                spot = Instantiate(AOESpotHealPrefab);
                target.SetParent(spot.transform);
                spot.transform.position = target.position + new Vector3(0, 0.05f, 0);
                spot.transform.rotation = Quaternion.Euler(90, 0, 0);
        } else {
            if (target != null) {
                // A healing target can be applied to an enemy or a friendly.
                spot = Instantiate(AOESpotHealPrefab);
                spot.transform.SetParent(target, true);
                spot.transform.position = target.position + new Vector3(0, 0.05f, 0);
                spot.transform.rotation = Quaternion.Euler(90, 0, 0);
            } else {
                // If no target is selected, drop it on self.
                spot = Instantiate(AOESpotHealPrefab);
                spot.transform.SetParent(caster, true);
                spot.transform.position = caster.position + new Vector3(0, 0.05f, 0);
                spot.transform.rotation = Quaternion.Euler(90, 0, 0);
                target = caster;
            }
        }

        spot.transform.GetComponent<AOESpot_Mono>().SetAOEDetails(caster, target, this, tickInterval, aoeSpotType, valuePerTick);
        Destroy(spot, duration);
        return true;
    }
}

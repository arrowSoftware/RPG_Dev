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

    public override bool Activate(Transform player, Transform target) {
        caster = player;
        this.target = target;
        GameObject explosion = Instantiate(AOESpotHealPrefab);
        explosion.transform.position = caster.position + new Vector3(0, 0.1f, 0);
        explosion.transform.GetComponent<AOESpot_Mono>().SetAOEDetails(tickInterval, aoeSpotType, valuePerTick);
        Destroy(explosion, duration);
        return true;
    }
}

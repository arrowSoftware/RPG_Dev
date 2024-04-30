using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AOE Explosion Ability", menuName = "Abilites/AOE (Explosion)")]
public class AOE_Explosion : Ability
{
    // https://www.youtube.com/watch?v=ode1-TwzNT0 
    Transform caster;
    Transform target;
    public GameObject explosionPrefab;

    public override bool Activate(Transform player, Transform target) {
        caster = player;
        this.target = target;
        GameObject explosion = Instantiate(explosionPrefab, caster);
        explosion.transform.position = caster.position + new Vector3(0, 1, 0);
        CheckForContacts();
        Destroy(explosion, 0.5f);
        return true;
    }

    void CheckForContacts() {
        Collider[] colliders = Physics.OverlapSphere(caster.position, 4.0f);
        foreach (Collider collider in colliders) {
            if (collider.GetComponent<CharacterStats>()) {
                collider.GetComponent<CharacterStats>().TakeDamage(50, this, false);
            }
        }
    }
}

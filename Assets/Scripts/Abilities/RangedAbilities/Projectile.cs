using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Reference
    // https://www.google.com/search?client=firefox-b-1-d&q=unity+projectile+follow+target#fpstate=ive&vld=cid:1000b734,vid:KobCxX7NZM4,st:0

    public AnimationCurve positionCurve;
    public AnimationCurve noiseCurve;
    private Coroutine homingCoroutine;
    public Vector2 minNoise = new Vector2(-3.0f, -0.25f);
    public Vector2 maxNoise = new Vector2(3.0f, 1.0f);
    public bool simple = true;

    private Transform target;
    private Transform caster;
    private float yOffset = 1.0f;
    private float moveSpeed = 1.0f;
    private Ability ability;
    private StatusEffectData statusEffect;
    private int damage;
    private bool splash;
    private float splashRadius;

    public void Spawn(Ability ability, StatusEffectData statusEffect, int damage, Transform caster, Transform target, float speed, bool splash = false, float splashRadius = 0.0f) {
        this.damage = damage;
        this.target = target;
        this.caster = caster;
        this.moveSpeed = speed;
        this.ability = ability;
        this.statusEffect = statusEffect;
        this.splash = splash;
        this.splashRadius = splashRadius;

        if (homingCoroutine != null) {
            StopCoroutine(homingCoroutine);
        }

        homingCoroutine = StartCoroutine(FindTarget());
    }

    private IEnumerator FindTarget() {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition, target.position);
        float remainingDistance = distance;

        while (remainingDistance > 0) {
            if (simple) {
                transform.position = Vector3.Lerp(startPosition, target.position + new Vector3(0, yOffset, 0), 1 - (remainingDistance / distance));
            } else {
                Vector3 noise = new Vector2(Random.Range(minNoise.x, maxNoise.x), Random.Range(minNoise.y, maxNoise.y));
                Vector3 projectileDirectionVector = new Vector3(target.position.x, target.position.y + yOffset, target.position.z) - startPosition;
                Vector3 horizontalNoiseVector = Vector3.Cross(projectileDirectionVector, Vector3.up).normalized;
                float noisePosition = noiseCurve.Evaluate(remainingDistance);
                transform.position = Vector3.Lerp(startPosition, target.position + new Vector3(0, yOffset, 0), 1 - (remainingDistance / distance)) + 
                    new Vector3(horizontalNoiseVector.x * noisePosition * noise.x, noisePosition * noise.y, noisePosition * horizontalNoiseVector.z * noise.x);
            }

            transform.LookAt(target.position + new Vector3(0, yOffset, 0));

            remainingDistance -= Time.deltaTime * moveSpeed;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform == target) {
            if (other.TryGetComponent<CharacterStats>(out CharacterStats stats)) {
                CharacterStats casterStats = caster.GetComponent<CharacterStats>();
                // If this is a splash projectile, find all characters within x radius of the target, and damage them as well.
                if (splash) {
                    List<CharacterStats> contacts = CheckForContacts();
                    for (int i = 0; i < contacts.Count; i++) {
                        if ((!casterStats.enemy && contacts[i].enemy) || (casterStats.enemy && !contacts[i].enemy)) {
                            contacts[i].TakeDamage(casterStats, damage, null);
                            var effectable = contacts[i].GetComponent<IEffectable>();
                            if (effectable != null) {
                                effectable.ApplyEffect(casterStats, statusEffect);
                            }
                        }
                    }
                } else {
                    stats.TakeDamage(casterStats, damage, null);
                    var effectable = target.GetComponent<IEffectable>();
                    if (effectable != null) {
                        effectable.ApplyEffect(casterStats, statusEffect);
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    List<CharacterStats> CheckForContacts() {
        List<CharacterStats> foundAttackables = new List<CharacterStats>();

        Collider[] colliders = Physics.OverlapSphere(target.position, splashRadius);
        foreach (Collider collider in colliders) {
            if (collider.TryGetComponent<CharacterStats>(out CharacterStats stats)) {
                foundAttackables.Add(stats);
            }
        }
        return foundAttackables;
    }
}


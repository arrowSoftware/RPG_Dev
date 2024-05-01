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
    private int damage;
    public Vector2 minNoise = new Vector2(-3.0f, -0.25f);
    public Vector2 maxNoise = new Vector2(3.0f, 1.0f);

    private Transform target;
    private Transform player;
    private float yOffset = 1.0f;
    private float moveSpeed = 1.0f;
    private Ability ability;
    private StatusEffectData statusEffect;

    public void Spawn(Ability ability, StatusEffectData statusEffect, int damage, Transform player, Transform target, float speed) {
        this.damage = damage;
        this.target = target;
        this.player = player;
        this.moveSpeed = speed;
        this.ability = ability;
        this.statusEffect = statusEffect;

        if (homingCoroutine != null) {
            StopCoroutine(homingCoroutine);
        }

        homingCoroutine = StartCoroutine(FindTarget());
    }

    private IEnumerator FindTarget() {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition, target.position);
        float remainingDistance = distance;
        Vector3 noise = new Vector2(Random.Range(minNoise.x, maxNoise.x), Random.Range(minNoise.y, maxNoise.y));
        Vector3 projectileDirectionVector = new Vector3(target.position.x, target.position.y + yOffset, target.position.z) - startPosition;
        Vector3 horizontalNoiseVector = Vector3.Cross(projectileDirectionVector, Vector3.up).normalized;

        while (remainingDistance > 0) {
            float noisePosition = noiseCurve.Evaluate(remainingDistance);
            transform.position = Vector3.Lerp(startPosition, target.position + new Vector3(0, yOffset, 0), 1 - (remainingDistance / distance)) + 
                new Vector3(horizontalNoiseVector.x * noisePosition * noise.x, noisePosition * noise.y, noisePosition * horizontalNoiseVector.z * noise.x);
            transform.LookAt(target.position + new Vector3(0, yOffset, 0));

            remainingDistance -= Time.deltaTime * moveSpeed;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform != player && other.transform == target) {
            Debug.Log("Contact");
            CharacterStats stats;
            if (other.TryGetComponent<CharacterStats>(out stats)) {
                stats.TakeDamage(damage, null, false);
                var effectable = target.GetComponent<IEffectable>();
                if (effectable != null) {
                    effectable.ApplyEffect(statusEffect);
                }
            }
            Destroy(gameObject);
        }
    }
}


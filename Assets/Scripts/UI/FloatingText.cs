using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    // TODO https://www.youtube.com/watch?v=_ICCSDmLCX4 better way to do this without creting so many game objects
    public float destroyTime = 1.0f;
    public Vector3 offset = new Vector3(0, 0, 0);
    public Vector3 randomizeIntensity = new Vector3(0.0f, 0, 0);
    private Animator anim;

    float smoothSpeed = 2.0f;
    float targetSpeed = 0.0f;
    float speedRef = 0.0f;

    Vector3 targetPoint;

    public void SetDetails(string text, bool crit, bool heal, bool enemy) {
        transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().SetText(text);

        if (crit) {
            transform.localScale *= 2;
        }

        if (enemy) {
            if (crit) {
                transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().color = Color.yellow;
            } else {
                transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().color = Color.white;
            }
        } else {
            // Self
            if (heal) {
                transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().color = Color.green;
            } else {
                transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().color = Color.red;
            }
        }
    }

    void Start()
    {
        Destroy(gameObject, destroyTime);
        transform.localPosition += offset;
        transform.localPosition += new Vector3(
            Random.Range(-randomizeIntensity.x, randomizeIntensity.x), 
            Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
            Random.Range(-randomizeIntensity.z, randomizeIntensity.z));
        transform.LookAt(Camera.main.transform.position);

        Quaternion randAng = Quaternion.Euler(0, Random.Range(-45,45), 0);
        randAng = transform.rotation * randAng;
        targetPoint = transform.position + randAng * Vector3.forward * 2;
        targetPoint.x = transform.position.x;
    }

    void Update() {
        smoothSpeed = Mathf.SmoothDamp(smoothSpeed, targetSpeed, ref speedRef, destroyTime/2);
        float step =  smoothSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);
        transform.LookAt(Camera.main.transform, Vector3.up);
    }
}

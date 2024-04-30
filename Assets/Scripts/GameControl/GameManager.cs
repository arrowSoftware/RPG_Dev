using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake() {
        instance = this;
    }

    public Transform notificationUI;
    bool notificationActive = false;

    void Update() {}

    public void SetWarning() {
        if (!notificationActive) {
            StartCoroutine(SendNotification("Out of Range", 3));
        }
    }

    IEnumerator SendNotification(string text, int duration) {
        notificationActive = true;
        notificationUI.GetChild(0).GetComponent<TMP_Text>().SetText(text);
        yield return new WaitForSeconds(duration);
        notificationUI.GetChild(0).GetComponent<TMP_Text>().SetText("");
        notificationActive = false;
    }
}

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
    public enum NotificationWarning {
        OutOfRange,
        InFrontOfTarget,
        OutOfEnergy,
        InvalidTarget
    }

    public void SetWarning(NotificationWarning warning) {
        if (!notificationActive) {
            string warningText = "";
            switch (warning) {
                case NotificationWarning.OutOfRange: {warningText = "Out of Range"; break;} 
                case NotificationWarning.InFrontOfTarget: {warningText = "Must be in front of target"; break;}
                case NotificationWarning.OutOfEnergy: {warningText = "Out of Energy"; break;}
                case NotificationWarning.InvalidTarget: {warningText = "Invalid Target"; break;}
            }
            StartCoroutine(SendNotification(warningText, 3));
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

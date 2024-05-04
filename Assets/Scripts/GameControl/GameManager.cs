using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Chat Window")]
    [SerializeField]
    List<ChatMessage> chatMessages= new List<ChatMessage>();
    public int maxChatMessage = 100;
    public GameObject chatMessageTextPrefab;
    public GameObject chatPanel;

#region Singleton
    public static GameManager instance;
    void Awake() {
        instance = this;
    }
#endregion

    [Header("Notifications")]
    public Transform notificationUI;
    bool notificationActive = false;
    public enum NotificationWarning {
        OutOfRange,
        InFrontOfTarget,
        OutOfEnergy,
        InvalidTarget
    }

    public void SendDamageMessage(string caster, Ability ability, string target, float damage, bool crit) {
        string damageText = "";
        if (crit) {
            damageText = "<color=orange>"+damage+"</color>";
        } else {
            damageText = "<color=red>"+damage+"</color>";
        }

        string text = caster + "'s <b><color=blue>" + ability.name + "</color></b> hits " + target + " for " + damageText + " damage";
        SendChatMessage(text);
    }

    public void SendHealingMessage(string caster, Ability ability, string target, float healing, bool crit) {
        string text = caster + "'s <b><color=blue>" + ability.name + "</color></b> heals " + target + " for <color=green>" + healing + "</color> amount";
        SendChatMessage(text);
    }

    public void SendChatMessage(string message) {
        if (chatMessages.Count >= maxChatMessage) {
            Destroy(chatMessages[0].textObject.gameObject);
            chatMessages.Remove(chatMessages[0]);
        }

        ChatMessage newMessage = new ChatMessage
        {
            text = message
        };

        GameObject newText = Instantiate(chatMessageTextPrefab, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<TMP_Text>();
        newMessage.textObject.text = newMessage.text;

        chatMessages.Add(newMessage);
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

[System.Serializable]
public class ChatMessage {
    public string text;
    public TMP_Text textObject;
}
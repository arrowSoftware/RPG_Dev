using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionBarUI : MonoBehaviour
{
    private List<Ability> activeAbilities = new List<Ability>(10);
    private List<ControlBinding> slotBindings = new List<ControlBinding>(10);
    Transform[] actionBarSlots = new Transform[10];
    public ActionBar actionBar;

    string GetKeyCodeString(KeyCode keyCode) {
        switch (keyCode) {
            case KeyCode.Alpha0: { return "0"; }
            case KeyCode.Alpha1: { return "1"; }
            case KeyCode.Alpha2: { return "2"; }
            case KeyCode.Alpha3: { return "3"; }
            case KeyCode.Alpha4: { return "4"; }
            case KeyCode.Alpha5: { return "5"; }
            case KeyCode.Alpha6: { return "6"; }
            case KeyCode.Alpha7: { return "7"; }
            case KeyCode.Alpha8: { return "8"; }
            case KeyCode.Alpha9: { return "9"; }
        }
        return keyCode.ToString();
    }

    void Start() {
        actionBar.OnActionBarChanged += OnActionBarChanged;
    }

    public void OnActionBarChanged(List<ControlBinding> slots, List<Ability> abilities) {
        slotBindings = slots;
        activeAbilities = abilities;

        for (int i = 0; i < 10; i++) {
            actionBarSlots[i] = transform.GetChild(i);
            TMP_Text keyBindText = actionBarSlots[i].GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            keyBindText.SetText(GetKeyCodeString(slotBindings[i].primary[0]));
 

            if (activeAbilities[i] != null) {
                actionBarSlots[i].GetChild(0).GetChild(0).GetComponent<Image>().sprite = activeAbilities[i].icon;
                actionBarSlots[i].GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(TargetSelection))]
public class TargetUnitframeUI : MonoBehaviour
{
    TargetSelection targetSelection;

    public Transform ui;
    public Transform target;
    [Header("Info Bar")]
    TMP_Text nameText;
    TMP_Text levelText;

    [Header("Health Bar")]
    Slider healthSlider;
    TMP_Text healthTextPercent;
    TMP_Text healthTextCurrent;
    float currentHealth = 100;
    float maxHealth = 100;

    [Header("Power Bar")]
    Slider powerSlider;
    TMP_Text powerTextPercent;
    TMP_Text powerTextCurrent;
    float currentPower = 100;
    float maxPower = 100;

    void Start()
    {
        ui.gameObject.SetActive(false);
        
        targetSelection = GetComponent<TargetSelection>();
        targetSelection.OnTargetSelected += OnTargetSelected;
    
        healthSlider = ui.GetChild(1).GetComponent<Slider>();
        healthTextPercent = ui.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        healthTextCurrent = ui.GetChild(1).GetChild(2).GetComponent<TMP_Text>();

        powerSlider = ui.GetChild(2).GetComponent<Slider>();
        powerTextPercent = ui.GetChild(2).GetChild(1).GetComponent<TMP_Text>();
        powerTextCurrent = ui.GetChild(2).GetChild(2).GetComponent<TMP_Text>();
   }

    void OnHealthChanged(float maxHealth, float currentHealth) {
        float healthPercent = (float)currentHealth / maxHealth;
        healthSlider.value = healthPercent;
        healthTextPercent.SetText((healthPercent * 100).ToString("f1") + "%");
        healthTextCurrent.SetText(((int)(healthPercent * maxHealth)).ToString());

        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
    }

    void OnPowerChanged(int maxPower, int currentPower) {
        float powerPercent = (float)currentPower / maxPower;
        powerSlider.value = powerPercent;
        powerTextPercent.SetText(((int)(powerPercent * 100)).ToString() + "%");
        powerTextCurrent.SetText(((int)(powerPercent * maxPower)).ToString()); 

        this.currentPower = currentPower;
        this.maxPower = maxPower;
    }

    void OnTargetSelected(Transform selection) {
        if (selection != null) {
            selection.GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
            //selection.GetComponent<CharacterStats>().OnPowerChanged += OnPowerChanged;

            nameText = ui.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            nameText.SetText(selection.name);
            ui.gameObject.SetActive(true);
        } else {
            nameText = ui.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            nameText.SetText("");
            ui.gameObject.SetActive(false);
        }
    }
}

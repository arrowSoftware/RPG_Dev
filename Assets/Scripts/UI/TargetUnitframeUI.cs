using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetUnitframeUI : MonoBehaviour
{
    TargetSelection targetSelection;

    public Transform ui;
    Transform currentSelection;

    [Header("Info Bar")]
    Transform infoBar;
    TMP_Text nameText;
    TMP_Text levelText;

    [Header("Health Bar")]
    Slider healthSlider;
    TMP_Text healthTextPercent;
    TMP_Text healthTextCurrent;
    float currentHealth = 100;
    float maxHealth = 100;

    void Start()
    {
        ui.gameObject.SetActive(false);
        
        targetSelection = GetComponent<TargetSelection>();
        targetSelection.OnTargetSelected += OnTargetSelected;
    
        healthSlider = ui.GetChild(0).GetComponent<Slider>();

        infoBar = ui.GetChild(1);
        nameText = infoBar.GetChild(1).GetComponent<TMP_Text>();
        levelText = infoBar.GetChild(0).GetComponent<TMP_Text>();

//        healthTextPercent = ui.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
//        healthTextCurrent = ui.GetChild(0).GetChild(2).GetComponent<TMP_Text>();
   }

    void OnHealthChanged(float maxHealth, float currentHealth) {
        float healthPercent = (float)currentHealth / maxHealth;
        healthSlider.value = healthPercent;
//        healthTextPercent.SetText((healthPercent * 100).ToString("f1") + "%");
//        healthTextCurrent.SetText(((int)(healthPercent * maxHealth)).ToString());

        this.currentHealth = currentHealth;
        this.maxHealth = maxHealth;
    }

    void SetFrameData(CharacterStats stats) {
        nameText.SetText(stats.name);
        levelText.SetText(stats.level.ToString());
        float healthPercent = (float)stats.currentHealth / stats.maxHealth;
        healthSlider.value = healthPercent;  
        this.currentHealth = stats.currentHealth;
        this.maxHealth = stats.maxHealth;

        if (stats.enemy) {
            // Set the selecion color to red
            ui.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.red;
        } else if (stats.npc) {
            // Set the color to yellow
            ui.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.yellow;
        } else {
            // Set the color to green.
            ui.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.green;
        }
    }

    void OnTargetSelected(Transform selection) {
        if (selection != currentSelection && currentSelection != null) {
            currentSelection.GetComponent<CharacterStats>().OnHealthChanged -= OnHealthChanged;
        }

        if (selection != null) {
            currentSelection = selection;
            selection.GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
            SetFrameData(selection.GetComponent<CharacterStats>());
            ui.gameObject.SetActive(true);
        } else {
            nameText.SetText("");
            levelText.SetText("");
            ui.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CharacterStats))]
public class PlayerUnitframeUI : MonoBehaviour
{
    public Transform ui;

    [Header("Health Bar")]
    Slider healthSlider;
    TMP_Text healthTextPercent;
    TMP_Text healthTextCurrent;

    [Header("Power Bar")]
    Slider powerSlider;
    TMP_Text powerTextPercent;
    TMP_Text powerTextCurrent;

    private CharacterStats player;

    void Start()
    {
        player = PlayerManager.instance.player.GetComponent<CharacterStats>();
        player.GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
        player.GetComponent<CharacterStats>().OnPowerChanged += OnPowerChanged;

        healthSlider = ui.GetChild(0).GetComponent<Slider>();
        healthTextPercent = ui.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        healthTextCurrent = ui.GetChild(0).GetChild(2).GetComponent<TMP_Text>();

        powerSlider = ui.GetChild(1).GetComponent<Slider>();
        powerTextPercent = ui.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        powerTextCurrent = ui.GetChild(1).GetChild(2).GetComponent<TMP_Text>();
   }

    void OnHealthChanged(float maxHealth, float currentHealth) {
        float healthPercent = currentHealth / maxHealth;
        healthSlider.value = healthPercent;
        healthTextPercent.SetText((healthPercent * 100).ToString("f1") + "%");
        string currentHealthText = ((int)(healthPercent * maxHealth)).ToString() + " / " + maxHealth;
        healthTextCurrent.SetText(currentHealthText);
    }

    void OnPowerChanged(float maxPower, float currentPower) {
        float powerPercent = currentPower / maxPower;
        powerSlider.value = powerPercent;
        powerTextPercent.SetText(((int)(powerPercent * 100)).ToString("f1") + "%");
        string currentPowerText = ((int)(powerPercent * maxPower)).ToString() + " / " + maxPower;
        powerTextCurrent.SetText(currentPowerText);
    }
}

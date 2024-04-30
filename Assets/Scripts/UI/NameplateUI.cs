using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CharacterStats))]
public class NameplateUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;
    float visibleTime = 10.0f; // Time to keep naeplate active after not taking damge.
    float lastMadeVisibleTime;
    public Canvas canvas;

    Transform ui;
    Slider healthSlider;
    Slider powerSlider;
    Transform cam;
    Transform statusEffectUI;
    public GameObject statusEffectPrefab;
    private List<StatusEffectStruct> statusEffectTracker = new List<StatusEffectStruct>(25);

    void Start()
    {
        cam = Camera.main.transform;

        ui = Instantiate(uiPrefab, canvas.transform).transform;
        healthSlider = ui.GetChild(0).GetComponent<Slider>();
        powerSlider = ui.GetChild(1).GetComponent<Slider>();

        ui.GetChild(2).GetComponent<TMP_Text>().SetText(transform.name);

        int level = transform.GetComponent<CharacterStats>().level;
        ui.GetChild(4).GetComponent<TMP_Text>().SetText(level.ToString());

        ui.gameObject.SetActive(false);

        statusEffectUI = ui.GetChild(3);
    
        GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
        GetComponent<CharacterStats>().OnPowerChanged += OnPowerChanged;
    }

    void LateUpdate()
    {
        if (ui != null) {
            ui.position = target.position;
            ui.forward = cam.forward;

            if (Time.time - lastMadeVisibleTime > visibleTime) {
                ui.gameObject.SetActive(false);
            }
        }
    }

    public void AddStatusEffect(StatusEffectStruct effect) {
        if (effect != null) {
            // If effect is already applied, refresh the time to max
            StatusEffectStruct check = statusEffectTracker.Find(x => x.statusEffectData.name == effect.statusEffectData.name);
            if (check != null) {
                check.currentEffectTime = effect.currentEffectTime;
                check.nextTickTime = effect.nextTickTime;
            } else {
                effect.statusEffectUiElement = Instantiate(statusEffectPrefab, statusEffectUI);
                effect.statusEffectUiElement.transform.GetChild(0).GetComponent<Image>().sprite = effect.statusEffectData.icon;
                effect.statusEffectUiElement.transform.GetChild(1).GetComponent<TMP_Text>().SetText(effect.statusEffectData.lifetime.ToString());
                statusEffectTracker.Add(effect);
            }
        }
    }

    public void RemoveStatusEffect(StatusEffectStruct effect) {
        StatusEffectStruct check = statusEffectTracker.Find(x => x.statusEffectData.name == effect.statusEffectData.name);

        // If the effect was found, clear it
        if (check != null) {
            if (check.statusEffectData.EffectParticles != null) {
                Destroy(check.statusEffectData.EffectParticles);
            }
        
            if (check.statusEffectUiElement != null) {
                Destroy(check.statusEffectUiElement);
            }
            statusEffectTracker.Remove(check);
        }
    }

    void OnHealthChanged(float maxHealth, float currentHealth) {
        if (ui != null) {
            ui.gameObject.SetActive(true);
            lastMadeVisibleTime = Time.time;

            float healthPercent = (float)currentHealth / maxHealth;
            healthSlider.value = healthPercent;

            ui.GetChild(0).GetChild(1).GetComponent<TMP_Text>().SetText((healthPercent*100).ToString("f1") + "%");
            ui.GetChild(0).GetChild(2).GetComponent<TMP_Text>().SetText(currentHealth.ToString() + " / " + maxHealth.ToString());

            if (currentHealth <= 0) {
                Destroy(ui.gameObject);
            }
        }
    }

    void OnPowerChanged(float maxPower, float currentPower) {
        if (ui != null) {
            ui.gameObject.SetActive(true);
            lastMadeVisibleTime = Time.time;

            float powerPercent = currentPower / maxPower;
            powerSlider.value = powerPercent;

            ui.GetChild(1).GetChild(1).GetComponent<TMP_Text>().SetText((powerPercent*100).ToString("f1") + "%");
            ui.GetChild(1).GetChild(2).GetComponent<TMP_Text>().SetText(currentPower.ToString() + " / " + maxPower.ToString());
        }
    }

    public void UpdateStatusEffect(StatusEffectStruct effect) {
        StatusEffectStruct check = statusEffectTracker.Find(x => x.statusEffectData.name == effect.statusEffectData.name);
        if (check != null) {
            check.currentEffectTime = effect.currentEffectTime;
            check.statusEffectUiElement.transform.GetChild(1).GetComponent<TMP_Text>().SetText((check.statusEffectData.lifetime-(int)check.currentEffectTime).ToString());
        }
    }
}

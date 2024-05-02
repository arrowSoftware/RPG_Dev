using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

public class StatusEffectManager : MonoBehaviour, IEffectable
{
    // List of all status effect on the character.
    private List<StatusEffectStruct> statusEffectTracker = new List<StatusEffectStruct>(25);

    // Nameplate UI
    NameplateUI nameplateUI;

    // Unit frame UI
    public Transform unitFrameStatusEffectUI;

    // public status effect prefab
    public GameObject statusEffectPrefab;

    // character stats
    CharacterStats stats;

    AbilityManager abilityManager;

    void Start() {
        nameplateUI = GetComponent<NameplateUI>();
        stats = GetComponent<CharacterStats>();
        abilityManager = GetComponent<AbilityManager>();
    }

    void Update() {
        if (statusEffectTracker.Count != 0) {
            HandleEffect();
        }  
    }

    void CreateAndAddToUnitFrame(CharacterStats casterStats, StatusEffectData effect) {
        StatusEffectStruct newEffect = new StatusEffectStruct();
        newEffect.statusEffectData = effect;
        newEffect.casterStats = casterStats;
        if (unitFrameStatusEffectUI != null) {
            newEffect.statusEffectUiElement = Instantiate(statusEffectPrefab, unitFrameStatusEffectUI);
            newEffect.statusEffectUiElement.transform.GetChild(0).GetComponent<Image>().sprite = effect.icon;
            newEffect.statusEffectUiElement.transform.GetChild(1).GetComponent<TMP_Text>().SetText(effect.lifetime.ToString());
        }

        statusEffectTracker.Add(newEffect);

        if (newEffect.statusEffectData.EffectParticles) {
            Instantiate(newEffect.statusEffectData.EffectParticles, transform);
        }
    }

    void CreateAndAddToNameplate(StatusEffectData effect) {
        if (nameplateUI != null) {
            StatusEffectStruct newEffect = new StatusEffectStruct();
            newEffect.statusEffectData = effect;
            nameplateUI.AddStatusEffect(newEffect);
        }
    }
    
    public void ApplyEffect(CharacterStats casterStats, StatusEffectData effect) {
        // If the effect is valid.
        if (effect != null) {
            // If effect is already applied, refresh the time to max
            StatusEffectStruct check = statusEffectTracker.Find(x => x.statusEffectData.name == effect.name);

            // If an existing effect is found then refresh the time.
            if (check != null) {
                check.currentEffectTime = 0;
                check.nextTickTime = 0;
            } else {
                CreateAndAddToUnitFrame(casterStats, effect);
            }

            CreateAndAddToNameplate(effect);
        }
    }

    public void RemoveEffect(StatusEffectData effect) {
        StatusEffectStruct check = statusEffectTracker.Find(x => x.statusEffectData.name == effect.name);

        // If the effect was found, clear it
        if (check != null) {
            check.currentEffectTime = 0;
            check.nextTickTime = 0;

            if (check.statusEffectData.EffectParticles != null) {
                Destroy(check.statusEffectData.EffectParticles);
            }
        
            if (check.statusEffectUiElement != null) {
                Destroy(check.statusEffectUiElement);
            }
            statusEffectTracker.Remove(check);

            nameplateUI.RemoveStatusEffect(check);
        }
    }

    public void HandleEffect() {
        for (int i = 0; i < statusEffectTracker.Count; i++) {
            statusEffectTracker[i].currentEffectTime += Time.deltaTime;
            if (statusEffectTracker[i].statusEffectUiElement != null) {
                // TODO create unitframe handler script to break this out of here.
                statusEffectTracker[i].statusEffectUiElement.transform.GetChild(1).GetComponent<TMP_Text>().SetText((statusEffectTracker[i].statusEffectData.lifetime-(int)statusEffectTracker[i].currentEffectTime).ToString());
            }

            nameplateUI.UpdateStatusEffect(statusEffectTracker[i]);

            if (statusEffectTracker[i].currentEffectTime > statusEffectTracker[i].statusEffectData.lifetime) {
                statusEffectTracker[i].statusEffectData.Cleanup(transform);
                RemoveEffect(statusEffectTracker[i].statusEffectData);
            }
        }

        if (statusEffectTracker.Count == 0) {
            return;
        }        

        for (int i = 0; i < statusEffectTracker.Count; i++) {
            if (statusEffectTracker[i].currentEffectTime > statusEffectTracker[i].nextTickTime) {
                statusEffectTracker[i].nextTickTime += statusEffectTracker[i].statusEffectData.tickSpeed;
                statusEffectTracker[i].statusEffectData.Process(statusEffectTracker[i].casterStats.transform, transform);
            }
        }
    }
}

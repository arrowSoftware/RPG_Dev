using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour
{
    Controls playerControls;
    GameObject player;
    AbilityManager abilityManager;
    private List<ControlBinding> actionBarSlots = new List<ControlBinding>(10);
    public List<Ability> activeAbilities = new List<Ability>(10);
    public Transform actionBarUI;

    public event System.Action<List<ControlBinding>, List<Ability> > OnActionBarChanged;

    void Start()
    {
        player = PlayerManager.instance.player;
        playerControls = player.GetComponent<PlayerControls>().controls;
        abilityManager = player.GetComponent<AbilityManager>();
        abilityManager.SetActiveAbilities(activeAbilities);
    
        actionBarSlots.Add(playerControls.ActionBarSlot1);
        actionBarSlots.Add(playerControls.ActionBarSlot2);
        actionBarSlots.Add(playerControls.ActionBarSlot3);
        actionBarSlots.Add(playerControls.ActionBarSlot4);
        actionBarSlots.Add(playerControls.ActionBarSlot5);
        actionBarSlots.Add(playerControls.ActionBarSlot6);
        actionBarSlots.Add(playerControls.ActionBarSlot7);
        actionBarSlots.Add(playerControls.ActionBarSlot8);
        actionBarSlots.Add(playerControls.ActionBarSlot9);
        actionBarSlots.Add(playerControls.ActionBarSlot10);

        if (OnActionBarChanged != null) {
            OnActionBarChanged(actionBarSlots, activeAbilities);
        }
    }

    void ProcessAbilityCooldown(Ability ability, int slot) {
        TMP_Text cooldownText = actionBarUI.GetChild(slot).GetChild(0).GetChild(2).GetComponent<TMP_Text>();
        Image slotIcon = actionBarUI.GetChild(slot).GetChild(0).GetChild(0).GetComponent<Image>();
        Image GCDImage = actionBarUI.GetChild(slot).GetChild(0).GetChild(3).GetComponent<Image>();

        // If the ability is on cooldown, update the ui to represent.
        if (abilityManager.IsAbilityOnCooldown(ability) == AbilityManager.CooldownType.abilityCooldown) {
            float duration = abilityManager.GetAbilityCooldownTimer(ability);
            string durationText = ((int)duration).ToString();
            if (duration == 0.0) {
                return;
            } else if (duration < 1.0f) {
                durationText = duration.ToString("f1");
            }

            // Set the text
            cooldownText.SetText(durationText);

            // Change cooldown background to "greyed" out
            Color tempColor = slotIcon.color;
            tempColor.a = 0.1f;
            slotIcon.color = tempColor;
        } else if (abilityManager.IsAbilityOnCooldown(ability) == AbilityManager.CooldownType.globalCooldown) {
            float duration = abilityManager.GetAbilityCooldownTimer(ability);
            GCDImage.enabled = true;
            slotIcon.GetComponent<Slider>().value = duration;
        } else { // If not on cooldown.
            // If the ui has not already been cleared
            if (GCDImage.enabled) {
                GCDImage.enabled = false;
            }

            cooldownText.SetText("");
            Color tempColor = slotIcon.color;
            tempColor.a = 1.0f;
            slotIcon.color = tempColor;
        }
    }

    void ProcessAbilityAction(Ability ability, int slot, bool isKeyDown) {
        if (isKeyDown) {
            Vector3 tempScale = actionBarUI.GetChild(slot).GetChild(0).GetChild(0).localScale;
            tempScale.x = 1.1f;
            tempScale.y = 1.1f;
            actionBarUI.GetChild(slot).GetChild(0).GetChild(0).localScale = tempScale;
            abilityManager.HandleAbility(ability);
        } else {
            Vector3 tempScale = actionBarUI.GetChild(slot).GetChild(0).GetChild(0).localScale;
            tempScale.x = 1f;
            tempScale.y = 1f;
            actionBarUI.GetChild(slot).GetChild(0).GetChild(0).localScale = tempScale;
        }
    }

    void ProcessAbility(Ability ability, int slot, ControlBinding binding) {
        ProcessAbilityCooldown(ability, slot);

        if (binding.GetControlBindingDown()) {
            ProcessAbilityAction(ability, slot, true);
        }
        if (binding.GetControlBindingUp()) {
            ProcessAbilityAction(ability, slot, false);
        }
    }

    void Update()
    {        
        for (int i = 0; i < activeAbilities.Count; i++) {
            ProcessAbility(activeAbilities[i], i, actionBarSlots[i]);
        }
    }
}

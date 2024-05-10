using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles abilities and ability cooldowns.
public class AbilityManager : MonoBehaviour
{
    /* References
    *   https://www.youtube.com/watch?v=ry4I6QyPw4E
    *   https://www.youtube.com/watch?v=0HCOZo5N-t4
    *   https://www.youtube.com/watch?v=Jv9jGyIWelU&t=300s
    *   https://www.youtube.com/watch?v=k7whbYamd_w 
    */

    public GameObject AOESelectionPrefab;
    GameObject AOESelectionGameObject;
    AbilityCoolDown placedAbility;
    Transform tempPlacedTransform;

    // Castbar reference
    CastBarUI castbar;

    // Casted ability co routine
    Coroutine castedAbilityCR;

    // Used to get the OnTargetSelected event.
    TargetSelection targetSelection;
    // Current target of the abilities.
    public Transform target;

    // Are all abilties on the global cooldown.
    bool isOnGlobalCooldown = false;
    // Default global cooldown time.
    const float GCD_VALUE = 1.0f;
    // Curent global cooldown time.
    float globalCooldownTimer = 0.0f;

    // Cooldown related to stuns.
    bool isOnStunCooldown = false;
    // Current stun cooldown timer.
    float stunCooldownTimer = 0.0f;

    // Is ability actively being casted
    bool isAbilityActive = false;

    // Enum for the types of ability cooldowns.
    public enum CooldownType {
        globalCooldown,  // The Global cooldown
        abilityCooldown, // Individual ability cooldown.
        stunCooldown,    // Stun related cooldowns
        noCooldown       // No cooldown
    }
    
    // Class to map abilties to their cooldowns.
    [System.Serializable]
    class AbilityCoolDown {
        // The abilities cooldown timer.
        public float cooldownTimer;
        // Is the ability on cooldown.
        public bool isOnCooldown;
        // Current ability.
        public Ability ability;

        // Constructor.
        public AbilityCoolDown(float cooldownTimer, bool isOnCooldown, Ability ability) {
            this.cooldownTimer = cooldownTimer;
            this.isOnCooldown = isOnCooldown;
            this.ability = ability;
        }
    }

    // List of the active abilities for this entity.
    [SerializeField]
    private List<AbilityCoolDown> activeAbilities = new List<AbilityCoolDown>(10);

    // Sets the ability manager active ability list, called from the Game Manager.
    public void SetActiveAbilities(Ability ability) {
        activeAbilities.Add(new AbilityCoolDown(0.0f, false, ability));
    }
    public void SetActiveAbilities(List<Ability> abilities) {
        // Add all the abilties to the active ability list.
        for (int i = 0; i < abilities.Count; i++) {
            activeAbilities.Add(new AbilityCoolDown(0.0f, false, abilities[i]));
        }
    }

    // Returns whether an ability is on any cooldown.  
    // Priority list:
    //  stun-(ability>GCD), GCD,  (ability<GCD)
    public CooldownType IsAbilityOnCooldown(Ability ability) {
        // Check for the ability in the ability cooldown list.
        AbilityCoolDown abilityCooldown = activeAbilities.Find((x) => x.ability == ability);

        // If the ability was found in the active list.
        if (abilityCooldown != null) {
            // If the ability is on any cooldown.
            if (abilityCooldown.isOnCooldown || isOnGlobalCooldown || isOnStunCooldown) {
                // Check stun cooldown first.
                if (isOnStunCooldown) {
                    return CooldownType.stunCooldown;
                }
                
                // Check to see if the current cooldown time is greater that the GCD.
                // If its greater just return the aility cooldown type.
                if (abilityCooldown.cooldownTimer > GCD_VALUE && abilityCooldown.isOnCooldown) {
                    return CooldownType.abilityCooldown;
                }

                // If the current cooldown is less than the GCD and its on
                // global cooldown then return global cooldown.
                if (isOnGlobalCooldown) {
                    return CooldownType.globalCooldown;
                }

                // If its not on global cooldown, and the time is less than the GCD
                // then just return the ability cooldown.
                return CooldownType.abilityCooldown;
            }
        }

        return CooldownType.noCooldown;
    }

    // Returns the abilities current cooldown time.
    public float GetAbilityCooldownTimer(Ability ability) {
        // Check for the ability in the ability cooldown list.
        AbilityCoolDown abilityCooldown = activeAbilities.Find((x) => x.ability == ability);

        // If the ability was found in the active list.
        if (abilityCooldown != null) {
            switch (IsAbilityOnCooldown(ability)) {
                case CooldownType.abilityCooldown: return abilityCooldown.cooldownTimer;
                case CooldownType.globalCooldown: return globalCooldownTimer;
                case CooldownType.stunCooldown: return stunCooldownTimer;
                case CooldownType.noCooldown: return -1;
            }
        }

        return -1;
    }

    void Start() {
        castbar = CastBarUI.instance;
        // If there is a target selection script on this object then attach
        // the callback function.
        if (TryGetComponent<TargetSelection>(out targetSelection)) {
            targetSelection.OnTargetSelected += OnTargetSelected;
        }
    }

    void Update() {
        // Stun cooldown is highest priority
        if (isOnStunCooldown) {
            // Decrement the cooldown timer
            stunCooldownTimer -= Time.deltaTime;
            // Turn off the cooldown timer if it expired.
            if (stunCooldownTimer <= 0) {
                isOnStunCooldown = false;
                stunCooldownTimer = 0.0f;
            }
        }

        // Count global cooldown
        if (isOnGlobalCooldown) {
            // Decrement the cooldown timer
            globalCooldownTimer -= Time.deltaTime;
            // Turn off the cooldown timer if it expired.
            if (globalCooldownTimer <= 0) {
                isOnGlobalCooldown = false;
                globalCooldownTimer = GCD_VALUE;
            }
        }

        // For each active ability, check its cooldown.
        for (int i = 0; i < activeAbilities.Count; i++) {
            // If the ability is on cooldown, decrement the timer.
            if (activeAbilities[i].isOnCooldown) {
                activeAbilities[i].cooldownTimer -= Time.deltaTime;
                // If the cooldown expires, reset its time to its max duration and turn off the cooldown.
                if (activeAbilities[i].cooldownTimer <= 0) {
                    activeAbilities[i].isOnCooldown = false;
                    activeAbilities[i].cooldownTimer = activeAbilities[i].ability.cooldownTime;
                }
            }
        }

        if (AOESelectionGameObject) {
            Ray ray = Camera.main.ScreenPointToRay(SoftwareCursor.instance.GetCursorPosition());
            if (Physics.Raycast(ray, out RaycastHit rayCastHit)) {
                AOESelectionGameObject.transform.position = rayCastHit.point + new Vector3(0, 0.05f, 0);
                if (SoftwareCursor.instance.GetMouseButtonDown(0)) {
                    // Spawn a new empty gameobject at the hit point to use as the target.
                    // delete the spot when the ability ends.
                    GameObject tempObject = new GameObject("AOESelectionTarget");
                    tempObject.transform.position = AOESelectionGameObject.transform.position;
                    target = tempObject.transform;
                    castedAbilityCR = StartCoroutine(CastAbility(placedAbility));
                    Destroy(AOESelectionGameObject);
                }
            }
        }
    }

    public void CancelCurrentAbilityCast() {
        StopCoroutine(castedAbilityCR);
        isOnGlobalCooldown = false;
    }

    IEnumerator CastAbility(AbilityCoolDown abilityCooldown) {
        // When activating the ability it starts the global cooldown, if the 
        // ability is cancelled it clears the global cooldown, When the ability 
        // is casted there is no additional cooldown outside of the ability 
        // cooldown. Only applicable to abilities with cast bars.
        isOnGlobalCooldown = true;

        // Activate the cast bar UI.
        if (castbar) {
            castbar.StartCastbar(abilityCooldown.ability.castTime);
        }

        isAbilityActive = true;

        if (abilityCooldown.ability.castTime == 0) {
            yield return null;
        } else {
            yield return new WaitForSeconds(abilityCooldown.ability.castTime);            
        }

        bool activated = abilityCooldown.ability.Activate(transform, target);

        // If the ability was activated then start the ability cooldown timer
        if (activated) {
            abilityCooldown.cooldownTimer = abilityCooldown.ability.cooldownTime;
            abilityCooldown.isOnCooldown = true;
            isAbilityActive = false;
        } else {
            isOnGlobalCooldown = false;
            if (castbar) {
                castbar.StopCastbar();
            }
            isAbilityActive = false;
        }
    }

    public void HandleAbility(Ability newAbility) {
        // If ability has a cast time, start a timer here, and if the cast bar finished activate the ability. TODO
        
        // Find ability in active ability list
        AbilityCoolDown abilityCooldown = activeAbilities.Find((x) => x.ability == newAbility);

        // If this is a placed ability, we need to set the mouse cursor to the placing circle.
        // then wait for the mouse click to start the ability function, the target bein the click position.
        if (newAbility.placedAOE) {
            if (!abilityCooldown.isOnCooldown && !isOnGlobalCooldown && !isOnStunCooldown && !isAbilityActive) {
                AOESelectionGameObject = Instantiate(AOESelectionPrefab);
                placedAbility = abilityCooldown;
            }
        } else {
            // If the ability is not on a cooldown.
            if (!abilityCooldown.isOnCooldown && !isOnGlobalCooldown && !isOnStunCooldown && !isAbilityActive) {
                // If the ability has a cast time, start a sub routine to activate the ability after x seconds.
                castedAbilityCR = StartCoroutine(CastAbility(abilityCooldown));
            }
        }

    }

    // Callback function for when a target has been selected by the target selection
    // class
    void OnTargetSelected(Transform selection) {
        target = selection;
    }

    // Function used to set the stun duration.
    public void SetStunAbilityCooldown(float duration) {
        isOnStunCooldown = true;
        stunCooldownTimer = duration;
    }
}

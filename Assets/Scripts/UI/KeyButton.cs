using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyButton : MonoBehaviour
{
    ControlBinding key;
    public Ability ability;

    public void SetBind(ControlBinding key) {
        this.key = key;
    }

    void Start() {
        GetComponent<Button>().onClick.AddListener(() => ButtonActivated(ability));
    }

    void Update()
    {
        if (key != null) {
            if (key.GetControlBindingDown()) {
                GetComponent<Button>().onClick.Invoke();
            }            
        }
    }

    void ButtonActivated(Ability ability) {
        // Call the character ability handler with this ability.
        //PlayerManager.instance.player.GetComponent<AbilityManager>().HandleAbility(ability);
    }
}

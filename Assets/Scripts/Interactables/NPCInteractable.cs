using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class NPCInteractable : Interactable
{
    PlayerManager playerManager;
    CharacterStats myStats;

    void Start() {
        playerManager = PlayerManager.instance;
        myStats = GetComponent<CharacterStats>();
    }

    public override void Interact()
    {
        base.Interact();
    }
}

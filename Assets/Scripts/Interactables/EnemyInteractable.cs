using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class EnemyInteractable : Interactable
{
    PlayerManager playerManager;
    CharacterStats myStats;

    void Start() {
        playerManager = PlayerManager.instance;
        player = playerManager.player.transform;
        myStats = GetComponent<CharacterStats>();
    }

    public override void Interact()
    {
        base.Interact();
    }
}

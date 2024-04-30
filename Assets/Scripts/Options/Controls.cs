using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Controls
{
    public ControlBinding forwards;
    public ControlBinding backwards;
    public ControlBinding strafeLeft;
    public ControlBinding strafeRight;
    public ControlBinding rotateLeft;
    public ControlBinding rotateRight;
    public ControlBinding walk_run;
    public ControlBinding jump;
    public ControlBinding autorun;
    public ControlBinding sit;
    public ControlBinding inventory;

    [Header("Action Bar")]
    public ControlBinding ActionBarSlot1;
    public ControlBinding ActionBarSlot2;
    public ControlBinding ActionBarSlot3;
    public ControlBinding ActionBarSlot4;
    public ControlBinding ActionBarSlot5;
    public ControlBinding ActionBarSlot6;
    public ControlBinding ActionBarSlot7;
    public ControlBinding ActionBarSlot8;
    public ControlBinding ActionBarSlot9;
    public ControlBinding ActionBarSlot10;
}

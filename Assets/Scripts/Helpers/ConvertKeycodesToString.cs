using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertKeycodesToString
{
    Dictionary<KeyCode, String> keyNames = new Dictionary<KeyCode, String>();

    private void Awake() {
        foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
            keyNames.Add(k, k.ToString());

        // replace Alpha0, Alpha1, .. and Keypad0... with "0", "1", ...
        for (int i = 0; i < 10; i++){
            keyNames[(KeyCode)((int)KeyCode.Alpha0+i)] = i.ToString();
            keyNames[(KeyCode)((int)KeyCode.Keypad0+i)] = i.ToString();
        }

//        keyNames[KeyCode.Comma] = ",";
//        keyNames[KeyCode.Escape] = "Esc";        
    }

    public String GetKeyCodeString(KeyCode keyCode) {
        return keyNames[keyCode];
    }
}

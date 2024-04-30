using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControlBinding
{
    public KeyCode[] primary = new KeyCode[1];
    public KeyCode[] secondary = new KeyCode[1];
    bool unpressed = false;

    public bool GetControlBinding()
    {
        bool primaryPressed = false;
        bool secondaryPressed = false;

        // Primary
        if (primary.Length == 1)
        {
            if (Input.GetKey(primary[0]))
            {
                primaryPressed = true;
            }
            else if (primary.Length == 2)
            {
                if (Input.GetKey(primary[0]) && Input.GetKey(primary[1]))
                {
                    primaryPressed = true;
                }
            }
        }

        // Secondary
            if (secondary.Length == 1)
        {
            if (Input.GetKey(secondary[0]))
            {
                secondaryPressed = true;
            }
            else if (secondary.Length == 2)
            {
                if (Input.GetKey(secondary[0]) && Input.GetKey(secondary[1]))
                {
                    secondaryPressed = true;
                }
            }
        }

        // Check key bindings
        if (primaryPressed || secondaryPressed)
        {
            return true;
        }

        return false;        
    }

    public bool GetControlBindingDown()
    {
        bool primaryPressed = false;
        bool secondaryPressed = false;

        // Primary
        if (primary.Length == 1)
        {
            if (Input.GetKey(primary[0]))
            {
                primaryPressed = true;
            }
            else if (primary.Length == 2)
            {
                if (Input.GetKey(primary[0]) && Input.GetKey(primary[1]))
                {
                    primaryPressed = true;
                }
            }
        }

        // Secondary
            if (secondary.Length == 1)
        {
            if (Input.GetKey(secondary[0]))
            {
                secondaryPressed = true;
            }
            else if (secondary.Length == 2)
            {
                if (Input.GetKey(secondary[0]) && Input.GetKey(secondary[1]))
                {
                    secondaryPressed = true;
                }
            }
        }

        if (!unpressed)
        {
            if (primaryPressed || secondaryPressed)
            {
                unpressed = true;
                return true;
            }
        }
        else
        {
            if (!primaryPressed && !secondaryPressed)
            {
                unpressed = false;
            }
        }

        return false;
    }

    public bool GetControlBindingUp()
    {
        bool primaryReleased = false;
        bool secondaryReleased = false;

        // Primary
        if (primary.Length == 1)
        {
            if (Input.GetKeyUp(primary[0]))
            {
                primaryReleased = true;
            }
            else if (primary.Length == 2)
            {
                if (Input.GetKeyUp(primary[0]) && Input.GetKeyUp(primary[1]))
                {
                    primaryReleased = true;
                }
            }
        }

        // Secondary
        if (secondary.Length == 1)
        {
            if (Input.GetKeyUp(secondary[0]))
            {
                secondaryReleased = true;
            }
            else if (secondary.Length == 2)
            {
                if (Input.GetKeyUp(secondary[0]) && Input.GetKeyUp(secondary[1]))
                {
                    secondaryReleased = true;
                }
            }
        }

        if (primaryReleased || secondaryReleased)
        {
            unpressed = true;
            return true;
        }

        return false;
    }
}

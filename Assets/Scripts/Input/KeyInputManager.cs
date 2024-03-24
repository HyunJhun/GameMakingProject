using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputManager : MonoBehaviour
{
    private List<KeyCode> keys = new List<KeyCode>();
    private KeyCode currentInputKey;
    // Start is called before the first frame update
    void Start()
    {
        keys.Add(KeyCode.Z);
        keys.Add(KeyCode.X);
        keys.Add(KeyCode.C);
    }
    public KeyCode GetCurrentInputKey()
    {
        return currentInputKey;
    }

    public bool CheckSkillKeyInput()
    {
        currentInputKey = KeyCode.None;

        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                currentInputKey = key;
                break;
            }
        }
        if (currentInputKey == KeyCode.None) return false;
        return true;

    }
}

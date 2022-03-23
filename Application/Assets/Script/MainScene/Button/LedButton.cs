using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedButton : ButtonPrototype
{
    public static LedButton instance;
    void Start() {
        Debug.Log("child");
        if (instance == null) instance = this;
        isOn = false;
        base.Start();
    }
}

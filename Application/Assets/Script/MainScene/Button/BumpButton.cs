using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpButton : ButtonPrototype
{
    public static BumpButton instance;
    void Start() {
        if (instance == null) instance = this;
        isOn = false;
        base.Start();
    }
}

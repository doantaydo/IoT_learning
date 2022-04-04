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
    public void clicked() {
        if (!ManagerData.instance.curBumpState)
             M2MqttUnity.Examples.MQTTclient.instance.pubBump("ON");
        else M2MqttUnity.Examples.MQTTclient.instance.pubBump("OFF");
    }
}

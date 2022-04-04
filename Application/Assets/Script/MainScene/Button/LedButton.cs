using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedButton : ButtonPrototype
{
    public static LedButton instance;
    void Start() {
        if (instance == null) instance = this;
        isOn = false;
        base.Start();
    }
    public void clicked() {
        if (!ManagerData.instance.curLedState)
             M2MqttUnity.Examples.MQTTclient.instance.pubLed("ON");
        else M2MqttUnity.Examples.MQTTclient.instance.pubLed("OFF");
    }
}

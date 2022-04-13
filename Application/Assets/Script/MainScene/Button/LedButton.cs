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
    void Update() {
        if (this.flag) {
            if (this.state_bef != ManagerData.instance.curLedState) {
                flag = false;
                this.btn.SetActive(true);
            }
        }
    }
    public void clicked() {
        this.state_bef = ManagerData.instance.curLedState;
        this.flag = true;
        this.btn.SetActive(false);

        if (!ManagerData.instance.curLedState)
             M2MqttUnity.Examples.MQTTclient.instance.pubLed("ON");
        else M2MqttUnity.Examples.MQTTclient.instance.pubLed("OFF");
    }
}

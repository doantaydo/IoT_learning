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
    void Update() {
        if (this.flag) {
            if (this.state_bef != ManagerData.instance.curBumpState) {
                flag = false;
                this.btn.SetActive(true);
            }
        }
    }
    public void clicked() {
        this.state_bef = ManagerData.instance.curBumpState;
        this.flag = true;
        this.btn.SetActive(false);

        if (!ManagerData.instance.curBumpState)
             M2MqttUnity.Examples.MQTTclient.instance.pubBump("ON");
        else M2MqttUnity.Examples.MQTTclient.instance.pubBump("OFF");
    }
}

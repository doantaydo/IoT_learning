using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakeValue : MonoBehaviour
{
    int count;
    void Start()
    {
        count = 0;
    }
    void FixedUpdate()
    {
        if (count == 100) {
            float curTemp = (float)((int)(UnityEngine.Random.Range(-50f, 100f) * 100)) / 100;
            float curHumid = (float)((int)(UnityEngine.Random.Range(0f, 100f) * 100)) / 100;
            M2MqttUnity.Examples.MQTTclient.instance.pubStatus(curTemp, curHumid);
            Debug.Log("temp: " + curTemp + ", humid: " + curHumid);
            count = 0;
        }
        
        count++;
    }
}

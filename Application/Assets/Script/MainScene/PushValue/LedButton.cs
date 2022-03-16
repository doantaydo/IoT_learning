using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedButton : MonoBehaviour
{
    public static LedButton instance;
    bool isOn;
    public GameObject redLed, greenLed;
    void Start() {
        if (instance == null) instance = this;
        isOn = false;
    }
    public void updateState(bool state) {
        isOn = state;
        if (isOn) {
            redLed.SetActive(false);
            greenLed.SetActive(true);
            Debug.Log("Green");
        }
        else {
            redLed.SetActive(true);
            greenLed.SetActive(false);
            Debug.Log("Red");
        }
    }

    public void clicked()
    {
        isOn = !isOn;
        if (isOn) {
            redLed.SetActive(false);
            greenLed.SetActive(true);
            Debug.Log("Green");
        }
        else {
            redLed.SetActive(true);
            greenLed.SetActive(false);
            Debug.Log("Red");
        }
    }
}

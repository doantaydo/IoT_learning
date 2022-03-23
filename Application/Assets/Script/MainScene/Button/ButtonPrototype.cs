using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPrototype : MonoBehaviour
{
    protected bool isOn;
    public GameObject redLed, greenLed;
    bool check;
    int count;
    SpriteRenderer red_rend, green_rend;
    Color color1, color2;
    protected void Start() {
        red_rend = redLed.GetComponent<SpriteRenderer>();
        green_rend = greenLed.GetComponent<SpriteRenderer>();
        color1 = new Color(1f, 1f, 1f, 1f);
        color2 = new Color(0.7f, 0.7f, 0.7f, 1f);
        check = true;
        count = 0;
    }
    void FixedUpdate() {
        if (count == 5) {
            if (check) {
                red_rend.color = color1;
                green_rend.color = color1;
            }
            else {
                red_rend.color = color2;
                green_rend.color = color2;
            }
            check = !check;
            count = -1;
        }
        count++;
    }
    public void updateState(bool state) {
        isOn = state;
        if (isOn) {
            redLed.SetActive(false);
            greenLed.SetActive(true);
        }
        else {
            redLed.SetActive(true);
            greenLed.SetActive(false);
        }
    }

    public void clicked()
    {
        isOn = !isOn;
        if (isOn) {
            redLed.SetActive(false);
            greenLed.SetActive(true);
        }
        else {
            redLed.SetActive(true);
            greenLed.SetActive(false);
        }
    }
}

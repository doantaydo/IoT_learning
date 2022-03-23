using UnityEngine;
using UnityEngine.UI;

public class ManagerData : MonoBehaviour
{
    public Text tempField, humidField;
    float curTemp, curHumid;
    bool curLedState, curBumpState;
    int count = 0;

    void Start()
    {
        count = 50;
    }
    void updateValue() {
        // get data from server

        // fake value
        curLedState = false;
        curBumpState = false;
        curTemp = (float)((int)(Random.Range(-50f,100f) * 100)) / 100;
        curHumid = (int)(Random.Range(0f, 100f));

        // change in UI
        BumpButton.instance.updateState(curBumpState);
        LedButton.instance.updateState(curLedState);

        tempField.text = curTemp.ToString();
        humidField.text = curHumid.ToString();
    }
    
    void FixedUpdate()
    {
        if (count == 50) {
            count = -1;
            updateValue();
        }
        count++;
    }
}

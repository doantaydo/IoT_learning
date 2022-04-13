using UnityEngine;
using UnityEngine.UI;

public class RollEff : MonoBehaviour
{
    float amount = 0;
    Image img;
    public bool t = true;
    void Start() {
        img = GetComponent<Image>();
        img.fillAmount = amount;
    }

    void Update()
    {
        if (t == true) {
            amount += 0.002f;
            if (amount > 1f) amount = 0f;
            img.fillAmount = amount;
        }
        else {
            float v = ManagerData.instance.curHumid;
            img.fillAmount = v / 100;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effLedOnOff : MonoBehaviour
{
    bool check;
    int count;
    SpriteRenderer renderer;
    void Start() {
        renderer = GetComponent<SpriteRenderer>();
        check = true;
        count = 0;
    }

    void FixedUpdate()
    {
        if (count == 5) {
            if (check)
                renderer.color = new Color(1f, 1f, 1f, 1f);
            else
                renderer.color = new Color(0.7f, 0.7f, 0.7f, 1f);
            check = !check;
            count = -1;
        }
        count++;
    }
}

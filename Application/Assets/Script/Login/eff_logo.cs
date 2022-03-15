using System.Collections;
using UnityEngine;

public class eff_logo : MonoBehaviour
{
    float start_A;
    SpriteRenderer renderer;
    public GameObject inputZone;
    void Start()
    {
        start_A = 0f;
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(1f, 1f, 1f, start_A);
    }

    // Update is called once per frame
    void Update()
    {
        if (start_A < 1f) {
            start_A += 0.01f;
            renderer.color = new Color(1f, 1f, 1f, start_A);
            if (start_A > 0.9f) inputZone.SetActive(true);
        }
    }
}

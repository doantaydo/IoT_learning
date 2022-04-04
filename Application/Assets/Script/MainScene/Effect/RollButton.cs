using UnityEngine;

public class RollButton : MonoBehaviour
{
    float rot;
    void Start()
    {
        rot = 360f;
        GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, rot);
    }

    // Update is called once per frame
    void Update()
    {
        rot -= 0.5f;
        if (rot < 0) rot = 360;
        GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, rot);
    }
}

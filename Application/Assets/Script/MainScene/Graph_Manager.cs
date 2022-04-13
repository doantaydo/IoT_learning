using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph_Manager : MonoBehaviour
{
    int[] pos_x;
    float[] array_temp;
    float[] array_humid;
    public GameObject line_temp, line_humid;
    private LineRenderer lr_temp, lr_humid;
    int num;
    int count_update;
    void Start()
    {
        lr_temp = line_temp.GetComponent<LineRenderer>();
        lr_humid = line_humid.GetComponent<LineRenderer>();

        pos_x = new int[10];
        array_temp = new float[10];
        array_humid = new float[10];

        for (int i = 0; i < 10; i++){
            pos_x[i] = -70 * i;
            lr_temp.SetPosition(i, new Vector3(0f, 0f, 0f));
            lr_humid.SetPosition(i, new Vector3(0f, 0f, 0f));
            
            array_temp[i] = 0f;
            array_humid[i] = 0f;
        }

        num = 0;
        count_update = 50;
    }

    
    void FixedUpdate()
    {
        if (count_update == 0) {
            for (int i = 9; i > 0; i--) {
                array_temp[i] = array_temp[i - 1];
                array_humid[i] = array_humid[i - 1];
            }
            array_temp[0] = ManagerData.instance.curTemp;
            array_humid[0] = ManagerData.instance.curHumid;
            if (num < 10) num++;

            updateGraph();
            count_update = 50;
        }
        count_update--;
        
    }
    void updateGraph() {
        for (int i = 0; i < num; i++) {
            lr_temp.SetPosition(i, new Vector3(pos_x[i], (array_temp[i] * 2f), 0f));
            lr_humid.SetPosition(i, new Vector3(pos_x[i], (array_humid[i] * 4f) - 200f, 0f));
        }
        for (int i = num; i < 10; i++) {
            lr_temp.SetPosition(i, new Vector3(pos_x[num - 1], (array_temp[num - 1] * 2f), 0f));
            lr_humid.SetPosition(i, new Vector3(pos_x[num - 1], (array_humid[num - 1] * 4f) - 200f, 0f));
        }
    }
}

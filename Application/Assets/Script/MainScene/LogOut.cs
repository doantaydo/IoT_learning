using UnityEngine;
using UnityEngine.SceneManagement;

public class LogOut : MonoBehaviour
{
    public GameObject canvasLogOut;
    public GameObject canvasData;
    bool ifOpening;
    void Start() {
        ifOpening = false;
    }
    public void activeSureToLogOut() {
        canvasData.SetActive(ifOpening);
        ifOpening = !ifOpening;
        canvasLogOut.SetActive(ifOpening);
    }
    public void notLogOut() {
        activeSureToLogOut();
        //SceneManager.LoadScene(1);
    }
    public void logOut() {
        M2MqttUnity.Examples.MQTTclient.instance.OnDestroy();
        SceneManager.LoadScene(0);
    }
}

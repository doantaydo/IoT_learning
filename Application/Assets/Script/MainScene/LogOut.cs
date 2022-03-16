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
        SceneManager.LoadScene(1);
    }
    public void logOut() {
        SceneManager.LoadScene(0);
    }
}

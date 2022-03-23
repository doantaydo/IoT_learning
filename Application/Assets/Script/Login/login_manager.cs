using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class login_manager : MonoBehaviour
{
    static login_manager instance;
    public InputField bro_URI, username, pwd;
    string bro_uri_txt, user_txt, pwd_txt;
    void Start()
    {
        if (instance == null) instance = this;
        bro_uri_txt = PlayerPrefs.GetString("bro_uri_txt","mqttserver.tk");
        user_txt = PlayerPrefs.GetString("user_txt","bkiot");
        pwd_txt = PlayerPrefs.GetString("pwd_txt","12345678");
        printTxt();
    }

    void printTxt() {
        bro_URI.text = bro_uri_txt;
        username.text = user_txt;
        pwd.text = pwd_txt;
        Debug.Log(user_txt);
    }
    void SaveTxt() {
        PlayerPrefs.SetString("bro_uri_txt",bro_uri_txt);
        PlayerPrefs.SetString("user_txt", user_txt);
        PlayerPrefs.SetString("pwd_txt", pwd_txt);
    }
    void getInputField() {
        bro_uri_txt = bro_URI.text;
        user_txt = username.text;
        pwd_txt = pwd.text;
    }
    public void checkLogin() {
        getInputField();
        if (pwd_txt != "12345678") {
            Debug.Log("Fail Login!!");
            SceneManager.LoadScene(0);
        }
        else {
            Debug.Log("Login Success!!");
            SaveTxt();
            SceneManager.LoadScene(1);
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Observer;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private Button btn;

    private void Start()
    {
        btn.onClick.AddListener(OnLogin);
    }

    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }

    public void OnLogin()
    {
        SendLoginRequest();
    }

    private async void SendLoginRequest()
    {   
        string usernameStr = username.text;
        string passwordStr = password.text;
        Toast.instance.SetWaiting();
        
        var response = await ApiRequest.instance.SendLoginRequest(usernameStr, passwordStr);
        
        if (response.success)
        {
            
            this.PostEvent(EventID.OnLogin, response.accessToken);
            SceneManager.LoadScene("Home");
            Toast.instance.DisableAll();
        }
        else
        {
            Toast.instance.SetFail();
        }
    }
    
}

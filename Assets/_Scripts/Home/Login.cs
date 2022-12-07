using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Observer;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class Login : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private Button btn;
    
    string usernameStr;
    string passwordStr;

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
        usernameStr = username.text;
        passwordStr = password.text;
        
        if (usernameStr.IsNullOrEmpty())
        {
            ToastMessage.instance.Show("Please enter Username");
            return;
        }
        
        if (passwordStr.IsNullOrEmpty())
        {
            ToastMessage.instance.Show("Please enter password");
            return;
        }
        
        SendLoginRequest();
    }

    private async void SendLoginRequest()
    {   
        
        Toast.instance.SetWaiting();
        
        var response = await ApiRequest.instance.SendLoginRequest(usernameStr, passwordStr);
        
        if (response.success)
        {
            
            UserManager.instance.auth = response.accessToken;
            UserManager.instance.GetUserInfomation();
            SceneManager.LoadScene("Home");
            Toast.instance.DisableAll();
        }
        else
        {
            Toast.instance.SetFail(response.message);
        }
    }
    
}

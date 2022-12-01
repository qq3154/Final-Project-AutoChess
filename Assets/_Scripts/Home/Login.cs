using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        string usernameStr = username.text;
        string passwordStr = password.text;
        ApiRequest.instance.SendLoginRequest(usernameStr, passwordStr);
    }
    
}

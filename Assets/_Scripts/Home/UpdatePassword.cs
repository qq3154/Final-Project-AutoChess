using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class UpdatePassword : MonoBehaviour
{
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField passwordConfirm;
    [SerializeField] private Button btn;

    private string passwordStr;
    private string passwordConfirmStr;

    private void Start()
    {
        btn.onClick.AddListener(OnUpdatePassword);
    }

    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }

    public void SetData()
    {
        password.text = "";
        passwordConfirm.text = "";
    }

    public void OnUpdatePassword()
    {
        passwordStr = password.text;
        passwordConfirmStr = passwordConfirm.text;
        
        if (passwordStr.IsNullOrEmpty())
        {
            ToastMessage.instance.Show("Please enter new password");
            return;
        }
        if (passwordStr.Length < 8)
        {
            ToastMessage.instance.Show("Password must be at least 8 characters");
            return;
        }
        if (passwordConfirmStr.IsNullOrEmpty() || passwordConfirmStr != passwordStr)
        {
            ToastMessage.instance.Show("Password and confirm password does not match!");
            return;
        }
        
        SendUpdatePassword();
    }

    async void SendUpdatePassword()
    {
        Toast.instance.SetWaiting();
        var response = await ApiRequest.instance.SendUpdatePasswordRequest(passwordStr);
        
        if (response.success)
        {
            Toast.instance.SetSuccess(response.message);
        }
        else
        {
            Toast.instance.SetFail(response.message);
        }
    }
}
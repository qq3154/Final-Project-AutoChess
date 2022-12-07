using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class UpdateProfile : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField fullname;
    [SerializeField] private TMP_InputField email;
    [SerializeField] private Button btn;

    private string usernameStr;
    private string fullnameStr;
    private string emailStr;
    
    private void Start()
    {
        btn.onClick.AddListener(OnUpdateProfile);
    }

    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }

    public void SetData()
    {
        username.text = UserManager.instance.username;
        fullname.text = UserManager.instance.fullName;
        email.text = UserManager.instance.email;
    }

    public void OnUpdateProfile()
    {
        usernameStr = username.text;
        fullnameStr = fullname.text;
        emailStr = email.text;

        if (fullnameStr.IsNullOrEmpty())
        {
            ToastMessage.instance.Show("Please enter your full name");
            return;
        }
        
        if (emailStr.IsNullOrEmpty())
        {
            ToastMessage.instance.Show("Please enter your email");
            return;
        }
        
        SendUpdateProfileRequest();
    }

    async void SendUpdateProfileRequest()
    {
        Toast.instance.SetWaiting();
        var response = await ApiRequest.instance.SendUpdateProfileRequest(usernameStr, fullnameStr, emailStr);
        
        if (response.success)
        { 
            UserManager.instance.username = username.text;
            UserManager.instance.fullName =  fullname.text;
            UserManager.instance.email =  email.text;
            Toast.instance.SetSuccess(response.message);
        }
        else
        {
            Toast.instance.SetFail("Update fail!");
        }
    }
}

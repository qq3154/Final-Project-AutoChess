using System.Collections;
using System.Collections.Generic;
using Observer;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField fullname;
    [SerializeField] private TMP_InputField email;
    [SerializeField] private Button btn;
    
    private void Start()
    {
        btn.onClick.AddListener(OnRegister);
    }

    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }

    public void OnRegister()
    {
        SendRegisterRequest();
    }

    private async void SendRegisterRequest()
    {   
        string usernameStr = username.text;
        string passwordStr = password.text;
        string fullnameStr = fullname.text;
        string emailStr = email.text;
        
        Toast.instance.SetWaiting();
        
        var response = await ApiRequest.instance.SendRegisterRequest(usernameStr, passwordStr, fullnameStr, emailStr);
        
        if (response.success)
        {
            Toast.instance.SetSuccess();
        }
        else
        {
            Toast.instance.SetFail();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using Observer;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class Register : MonoBehaviour
{
    
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField passwordConfirm;
    [SerializeField] private TMP_InputField fullname;
    [SerializeField] private TMP_InputField email;
    [SerializeField] private Button btn;

    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMap;

    private string usernameStr;
    private string passwordStr;
    private string passwordConfirmStr;
    private string fullnameStr;
    private string emailStr;
    
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
        usernameStr = username.text;
        passwordStr = password.text;
        passwordConfirmStr = passwordConfirm.text;
        fullnameStr = fullname.text;
        emailStr = email.text;
        
        //simple validate
        if (usernameStr.IsNullOrEmpty())
        {
            ToastMessage.instance.Show("Please enter Username");
            return;
        }
        if (usernameStr.Length < 6)
        {
            ToastMessage.instance.Show("Username must be at least 6 characters");
            return;
        }
        
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
        if (passwordStr.IsNullOrEmpty())
        {
            ToastMessage.instance.Show("Please enter Password");
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

        SendRegisterRequest();
    }

    private async void SendRegisterRequest()
    {
        Toast.instance.SetWaiting();
        
        var response = await ApiRequest.instance.SendRegisterRequest(usernameStr, passwordStr, fullnameStr, emailStr);
        
        if (response.success)
        {
            Toast.instance.SetSuccess("Register successful!");
            
            //create cards
            UserManager.instance.auth = response.accessToken;
            
            var configs = _heroProfileConfigMap;
            foreach (var config in configs.list)
            {
                string name = config.heroConfig.HeroStats.Name;
                string description = config.heroConfig.HeroStats.Description;
                int level = 1;
                int maxLevel = config.heroConfig.HeroStats.MaxLevel;

                CreateCard(name, description, level, maxLevel);
            }
        }
        else
        {
            Toast.instance.SetFail(response.message);
        }
    }
    
    public void CreateCard(string name, string description, int level, int maxLevel)
    {
        CreateCardRequest(name, description, level, maxLevel);
    }
    
    private async void CreateCardRequest(string name, string description, int level, int maxLevel)
    {
        var response = await ApiRequest.instance.SendCreateCardRequest(name, description, level, maxLevel);
        
        if (response.success)
        {
            Debug.Log("Create cards successful!");
        }
        else
        {
            Debug.LogError("Get user profile fail");
        }
    }

}

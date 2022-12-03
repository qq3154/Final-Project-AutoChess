using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Observer;

public class UserManager : MonoSingleton<UserManager>
{
    public string auth;
    public string id;
    public string username;
    public string password;
    public string fullName;
    public string email;
    public int gold;
    
    protected override void DoOnAwake()
    {
        base.DoOnAwake();
        this.RegisterListener(EventID.OnLogin, (param) => DoOnLogin((string) param));
    }

    private void DoOnLogin( string auth)
    {
        this.auth = auth;
        SendLoginRequest();
    }
    
    private async void SendLoginRequest()
    {
        var response = await ApiRequest.instance.SendGetUserProfileRequest();
        
        if (response.success)
        {
            SetUserProfile(response.user._id, response.user.username, response.user.password, response.user.fullName, response.user.email, response.user.gold);
        }
        else
        {
            Debug.LogError("Get user profile fail");
        }
    }

    public void SetUserProfile(string id, string username, string password, string fullName, string email, int gold )
    {
        this.id = id;
        this.username = username;
        this.password = password;
        this.fullName = fullName;
        this.email = email;
        this.gold = gold;
    }
}

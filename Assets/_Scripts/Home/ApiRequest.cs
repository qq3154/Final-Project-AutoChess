using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Observer;

public class ApiRequest : MonoSingleton<ApiRequest>
{
    [SerializeField] private string baseUrl = "https://axie-tactic-back-end.herokuapp.com";

    public void SendLoginRequest(string username, string password)
    {
        StartCoroutine( IE_SendLoginRequest(username, password));
    }
    
    public void SendRegisterRequest(string username, string password, string fullName, string email)
    {
        StartCoroutine( IE_SendRegisterRequest(username, password, fullName, email));
    }
    
    public void SendGetUserProfileRequest()
    {
        StartCoroutine( IE_SendGetUserProfileRequest());
    }
    
    public void SendUpdateProfileRequest(string username, string fullName, string email)
    {
        StartCoroutine( IE_SendUpdateProfileRequest(username, fullName, email));
    }
    
    IEnumerator  IE_SendLoginRequest(string username, string password)
    {
        string url = baseUrl + "/api/auth/login/";
        
        ParamRequest param = new ParamRequest();
        param.username = username;
        param.password = password;
        string json = JsonConvert.SerializeObject(param);
        
        UnityWebRequest request= new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        byte[] bytes= Encoding.UTF8.GetBytes(json);
        UploadHandlerRaw uH= new UploadHandlerRaw(bytes);
        uH.contentType= "application/json";
        request.uploadHandler= uH;
        request.downloadHandler = new DownloadHandlerBuffer();
        
        Toast.instance.SetWaiting();
        yield return request.SendWebRequest();
        var response = JsonConvert.DeserializeObject<ResponseHandler>(request.downloadHandler.text);
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
    
    IEnumerator  IE_SendRegisterRequest(string username, string password, string fullName, string email)
    {
        string url = baseUrl + "/api/auth/register/";
        
        ParamRequest param = new ParamRequest();
        param.username = username;
        param.password = password;
        param.fullName = fullName;
        param.email = email;
        param.role = 1;
        param.gold = 0;
        string json = JsonConvert.SerializeObject(param);
        
        UnityWebRequest request= new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        byte[] bytes= Encoding.UTF8.GetBytes(json);
        UploadHandlerRaw uH= new UploadHandlerRaw(bytes);
        uH.contentType= "application/json";
        request.uploadHandler= uH;
        request.downloadHandler = new DownloadHandlerBuffer();
        
        Toast.instance.SetWaiting();
        yield return request.SendWebRequest();
        var response = JsonConvert.DeserializeObject<ResponseHandler>(request.downloadHandler.text);
        if (response.success)
        {
            //SceneManager.LoadScene("Home");
            Toast.instance.DisableAll();
        }
        else
        {
            Toast.instance.SetFail();
        }
    }
    
    IEnumerator  IE_SendGetUserProfileRequest()
    {
        string url = baseUrl + "/api/user/";
        string auth = UserManager.instance.auth;

        UnityWebRequest request= new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + auth);
        
        Toast.instance.SetWaiting();
        yield return request.SendWebRequest();
        var response = JsonConvert.DeserializeObject<ResponseHandler>(request.downloadHandler.text);
        if (response.success)
        {
            //SceneManager.LoadScene("Home");
            Toast.instance.DisableAll();
            UserManager.instance.SetUserProfile(response.user._id, response.user.username, response.user.password, response.user.fullName, response.user.email, response.user.gold);
        }
        else
        {
            Toast.instance.SetFail();
        }
    }
    
    IEnumerator  IE_SendUpdateProfileRequest(string username, string fullName, string email)
    {
        string url = baseUrl + "/api/user/update/";
        string auth = UserManager.instance.auth;
        
        ParamRequest param = new ParamRequest();
        param.username = username;
        param.fullName = fullName;
        param.email = email;
        string json = JsonConvert.SerializeObject(param);
        
        UnityWebRequest request= new UnityWebRequest(url, UnityWebRequest.kHttpVerbPUT);
        byte[] bytes= Encoding.UTF8.GetBytes(json);
        UploadHandlerRaw uH= new UploadHandlerRaw(bytes);
        uH.contentType= "application/json";
        request.uploadHandler= uH;
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + auth);
        
        Toast.instance.SetWaiting();
        yield return request.SendWebRequest();
        var response = JsonConvert.DeserializeObject<ResponseHandler>(request.downloadHandler.text);
        if (response.success)
        {
            Toast.instance.DisableAll();
        }
        else
        {
            Toast.instance.SetFail();
        }
    }
 
    
    public struct ParamRequest
    {
        public string auth;
        public string username;
        public string password;
        public string fullName;
        public string email;
        public int role;
        public int gold;
    }
    public struct ResponseHandler
    {
        public bool success;
        public string message;
        public string accessToken;
        public UserResponseHandler user;
    }
    
    public struct UserResponseHandler
    {
        public string _id;
        public string username;
        public string password;
        public string fullName;
        public string email;
        public int gold;
            
    }
}





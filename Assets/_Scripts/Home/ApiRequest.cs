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
    //[SerializeField] private string baseUrl = "https://axie-tactic-back-end.herokuapp.com";
    [SerializeField] private string baseUrl = "http://localhost:5000";

    public async Task<ResponseHandler> SendLoginRequest(string username, string password)
    {
        string url = baseUrl + "/api/auth/login/";
        
        ParamRequest param = new ParamRequest();
        param.username = username;
        param.password = password;
        string json = JsonConvert.SerializeObject(param);
       
        
        var result = await SendRequestAsync(url, json, UnityWebRequest.kHttpVerbPOST, false );
        return result;
    }
    
    public async Task<ResponseHandler> SendRegisterRequest(string username, string password, string fullName, string email)
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
       
        
        var result = await SendRequestAsync(url, json, UnityWebRequest.kHttpVerbPOST, false );
        return result;
    }
    
    public async Task<ResponseHandler>  SendGetUserProfileRequest()
    {
        string url = baseUrl + "/api/user/";

        var result = await SendRequestAsync(url, null, UnityWebRequest.kHttpVerbGET, true );
        return result;
    }
    
    public async Task<ResponseHandler>  SendUpdateProfileRequest(string username, string fullName, string email)
    {
        string url = baseUrl + "/api/user/update/";
        
        ParamRequest param = new ParamRequest();
        param.username = username;
        param.fullName = fullName;
        param.email = email;
        string json = JsonConvert.SerializeObject(param);
        
        var result = await SendRequestAsync(url, json, UnityWebRequest.kHttpVerbPUT, true );
        return result;
    }
    
    public async Task<ResponseHandler>  SendGetMatchesRequest()
    {
        string url = baseUrl + "/api/match/";
        
        
        var result = await SendRequestAsync(url, null, UnityWebRequest.kHttpVerbGET, true );
        return result;
    }
    
    private async Task<ResponseHandler> SendRequestAsync(string url, string param, string requestType, bool isAuthentication )
    {

        UnityWebRequest request= new UnityWebRequest(url, requestType);


        if (param != null)
        {
            byte[] bytes= Encoding.UTF8.GetBytes(param);
            UploadHandlerRaw uH= new UploadHandlerRaw(bytes);
            uH.contentType= "application/json";
            request.uploadHandler= uH;
        }
       
        
        request.downloadHandler = new DownloadHandlerBuffer();

        if (isAuthentication)
        {
            string auth = UserManager.instance.auth;
            request.SetRequestHeader("Authorization", "Bearer " + auth);
        }
        
        request.SendWebRequest();
        while (!request.isDone)
        {
            await Task.Yield();
        }
        var response = JsonConvert.DeserializeObject<ResponseHandler>(request.downloadHandler.text);
        return response;
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
        public List<MatchesResponseHandler> matches;
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
    
    public struct MatchesResponseHandler
    {
        public string _id;
        public UserResponseHandler winner;
        public UserResponseHandler loser;
        public int round;
        public string createAt;
    }
    
    
}




using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ApiRequest : MonoSingleton<ApiRequest>
{
    [SerializeField] private string baseUrl = "https://axie-tactic-back-end.herokuapp.com";

    public void SendLoginRequest(string username, string password)
    {
        StartCoroutine( IE_SendLoginRequest(username, password));
    }

   
    IEnumerator  IE_SendLoginRequest(string username, string password)
    {
        
        ParamRequest param = new ParamRequest();
        param.username = username;
        param.password = password;
        string json = JsonConvert.SerializeObject(param);
        Debug.Log(json);
        
        string url = baseUrl + "/api/auth/login/";
        
        UnityWebRequest request= new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        byte[] bytes= GetBytes(json);
        UploadHandlerRaw uH= new UploadHandlerRaw(bytes);
        uH.contentType= "application/json";
        request.uploadHandler= uH;
        request.downloadHandler = new DownloadHandlerBuffer();
        
        yield return request.SendWebRequest();
        var response = JsonConvert.DeserializeObject<ResponseHandler>(request.downloadHandler.text);
        if (response.success)
        {
            SceneManager.LoadScene("Home");
        }
        else
        {
            Debug.Log("Incorrect Username/Password");
        }
    }
    
    UnityWebRequest CreateUnityWebRequest(string url, string param) {
        UnityWebRequest requestU= new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        byte[] bytes= GetBytes(param);
        UploadHandlerRaw uH= new UploadHandlerRaw(bytes);
        uH.contentType= "application/json";
        requestU.uploadHandler= uH;
        //requestU.SetRequestHeader("Content-Type", "application/json");
        //requestU.SetRequestHeader("Authorization", "TOKEN " + "yourAPIKey");
        CastleDownloadHandler dH= new CastleDownloadHandler();
        requestU.downloadHandler= dH; //need a download handler so that I can read response data
        return requestU;
    }
    
    class CastleDownloadHandler: DownloadHandlerScript {
        public delegate void Finished();
        public event Finished onFinished;
 
        protected override void CompleteContent ()
        {
            UnityEngine.Debug.Log("CompleteContent()");
            base.CompleteContent ();
            if (onFinished!= null) {
                onFinished();
            }
        }
    }
    protected static byte[] GetBytes(string str){
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        return bytes;
    }
    
   
    
    public class ParamRequest
    {
        public string username;
        public string password; 
    }

    public class ResponseHandler
    {
        public bool success;
        public string msg;
        public string accessToken;
    }
}





using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class HTTPMethod : MonoBehaviour
{
   
   [SerializeField] private TMP_Text msgTxt;
   [SerializeField] private String url;
   
   public void GetMethod()
   {
      StartCoroutine(IE_GetMethod());
   }

   public void PostMethod()
   {
      StartCoroutine( IE_PostMethod());
   }

   public void PutMethod()
   {
      StartCoroutine( IE_PutMethod());
   }

   IEnumerator IE_GetMethod()
   {
      string url = "https://qq3154-book-store.herokuapp.com/v1/author/";
      using (UnityWebRequest request = UnityWebRequest.Get(url))
      {
         yield return request.SendWebRequest();
         if (request.result == UnityWebRequest.Result.ConnectionError)
         {
            msgTxt.text = request.error;
         }
         else
         {
            msgTxt.text = request.downloadHandler.text;
         }
      }
   }
   
   IEnumerator IE_PostMethod()
   {
      Author author = new Author();
      author.name = "Hoang Quan";
      author.age = 14;
      string json = JsonConvert.SerializeObject(author);
      Debug.Log(json);
      
      
      string url = "https://qq3154-book-store.herokuapp.com/v1/author/";
      UnityWebRequest request = CreateUnityWebRequest(url, json);
      yield return request.SendWebRequest();
      if (request.result == UnityWebRequest.Result.ConnectionError)
      {
         Debug.Log(request.error);
      }
      else
      {
         Debug.Log(request.downloadHandler.text);
      }
      
      // using (UnityWebRequest request2 = UnityWebRequest.Post(url,json))
      // {
      //    request.SetRequestHeader("Content-Type", "application/json");
      //    yield return request.SendWebRequest();
      //    if (request.result == UnityWebRequest.Result.ConnectionError)
      //    {
      //       msgTxt.text = request.error;
      //    }
      //    else
      //    {
      //       msgTxt.text = request.downloadHandler.text;
      //    }
      // }
   }
   
   IEnumerator IE_PutMethod()
   {
      Author author = new Author();
      author.name = "Hoang Quan updated";
      author.age = 14;
      string json = JsonConvert.SerializeObject(author);
      Debug.Log(json);
      
      
      string url = "https://qq3154-book-store.herokuapp.com/v1/author/63821ec2b6c80eb9a1d695d8";
      UnityWebRequest request = CreateUnityWebRequest2(url, json);
      yield return request.SendWebRequest();
   }
   
   UnityWebRequest CreateUnityWebRequest(string url, string param) {
      UnityWebRequest requestU= new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
      byte[] bytes= GetBytes(param);
      UploadHandlerRaw uH= new UploadHandlerRaw(bytes);
      uH.contentType= "application/json";
      requestU.uploadHandler= uH;
      requestU.SetRequestHeader("Content-Type", "application/json");
      requestU.SetRequestHeader("Authorization", "TOKEN " + "yourAPIKey");
      CastleDownloadHandler dH= new CastleDownloadHandler();
      requestU.downloadHandler= dH; //need a download handler so that I can read response data
      return requestU;
   }
   
   UnityWebRequest CreateUnityWebRequest2(string url, string param) {
      UnityWebRequest requestU= new UnityWebRequest(url, UnityWebRequest.kHttpVerbPUT);
      byte[] bytes= GetBytes(param);
      UploadHandlerRaw uH= new UploadHandlerRaw(bytes);
      uH.contentType= "application/json";
      requestU.uploadHandler= uH;
      requestU.SetRequestHeader("Content-Type", "application/json");
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
   
   public class Author
   {
      public string name;
      public int age; 
   }

}

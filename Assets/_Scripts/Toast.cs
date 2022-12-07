using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Toast : MonoSingleton<Toast>
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _bg;
    [SerializeField] private GameObject wating;
    [SerializeField] private GameObject success;
    [SerializeField] private GameObject fail;
    [SerializeField] private TMP_Text failMsg;
    [SerializeField] private TMP_Text successMsg;
 

    private void GetCamera()
    {
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        _canvas.worldCamera = cam;
    }

    public void SetWaiting()
    {
        GetCamera();
        _bg.gameObject.SetActive(true);
        wating.SetActive(true);
        success.SetActive(false);
        fail.SetActive(false);
    }

    public void SetSuccess(string msg = null)
    {
        GetCamera();
        _bg.gameObject.SetActive(true);
        wating.SetActive(false);
        success.SetActive(true);
        fail.SetActive(false);
        successMsg.text = msg;
    }
    
    public void SetFail(string msg = null)
    {
        GetCamera();
        _bg.gameObject.SetActive(true);
        wating.SetActive(false);
        success.SetActive(false);
        fail.SetActive(true);
        failMsg.text = msg;
    }
    
    public void DisableAll()
    {
        _bg.gameObject.SetActive(false);
        wating.SetActive(false);
        success.SetActive(false);
        fail.SetActive(false);
    }
    

    
}

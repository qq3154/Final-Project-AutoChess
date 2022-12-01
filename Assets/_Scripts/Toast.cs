using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Toast : MonoSingleton<Toast>
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _bg;
    [SerializeField] private GameObject wating;
    [SerializeField] private GameObject success;
    [SerializeField] private GameObject fail;

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

    public void SetSuccess()
    {
        GetCamera();
        _bg.gameObject.SetActive(true);
        wating.SetActive(false);
        success.SetActive(true);
        fail.SetActive(false);
    }
    
    public void SetFail()
    {
        GetCamera();
        _bg.gameObject.SetActive(true);
        wating.SetActive(false);
        success.SetActive(false);
        fail.SetActive(true);
    }
    
    public void DisableAll()
    {
        _bg.gameObject.SetActive(false);
        wating.SetActive(false);
        success.SetActive(false);
        fail.SetActive(false);
    }
    

    
}

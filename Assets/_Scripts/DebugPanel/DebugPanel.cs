using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private GameObject _content;
    [SerializeField] private Button _openBtn;
    [SerializeField] private float _openWidth;
    [SerializeField] private float _openHeight;
    [SerializeField] private float _closeWidth;
    [SerializeField] private float _closeHeight;
    [SerializeField] private bool isOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        ClosePanel();
        
    }


    private void OnEnable()
    {
        _openBtn.onClick.AddListener(OpenClosePanel);
    }

    public void OpenClosePanel()
    {
        if (isOpen)
        {
            ClosePanel();
        }
        else
        {
            OpenPanel();
        }
    }

    private void ClosePanel()
    {
        _root.GetComponent<RectTransform>().sizeDelta = new Vector2(_closeWidth, _closeHeight);
        _content.SetActive(false);
        isOpen = false;
    }
    
    private void OpenPanel()
    {
        _root.GetComponent<RectTransform>().sizeDelta = new Vector2(_openWidth, _openHeight);
        _content.SetActive(true);
        isOpen = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsReadyToFindMatch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button _btn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        _btn.interactable = GameFlowManager.instance.isConnect;

    }
}

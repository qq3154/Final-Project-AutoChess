using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class getPlayerId : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = PhotonNetwork.LocalPlayer.ActorNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
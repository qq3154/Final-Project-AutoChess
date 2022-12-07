using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Observer;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GamePlayManager : MonoBehaviour, IOnEventCallback
{
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SendOngameStart();
        }
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    
    private void SendOngameStart()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
        PhotonNetwork.RaiseEvent(PhotonEvent.OnGameplayStart, null, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonEvent.OnGameplayStart)
        {
            StartCoroutine(IE_Delay());
        }
    }
    
    IEnumerator IE_Delay()
    {
        yield return new WaitForSeconds(1);
        //this.PostEvent(EventID.OnSelectCardPhaseStart);
       
        if (PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(PhotonEvent.OnSelectCardPhaseStart, null, raiseEventOptions, SendOptions.SendReliable);
        }
        
    }
}

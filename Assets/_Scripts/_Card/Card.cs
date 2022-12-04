using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Game;
using Observer;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private string _heroID;

    [SerializeField] private Button _btn;
    
    [SerializeField] private AxieSpawner _axieSpawner;

    [SerializeField] private int _id;

    private void Awake()
    {
        _btn.onClick.AddListener(() => OnSelectCard());
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveAllListeners();
    }
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void InitCard(int id, string heroID)
    {
        _id = id;
        _heroID = heroID;
        _axieSpawner.Init(heroID);
    }

    public void OnSelectCard()
    {
       
        //BoardManager.instance.AddHero(teamID, _heroID, this);
        
        object[] content = new object[] {_id, GameFlowManager.instance.playerTeam}; // Array contains the target position and the IDs of the selected units
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(PhotonEvent.OnSelectCard, content, raiseEventOptions, SendOptions.SendReliable);
        
    }

    public void SetInteractable(bool isInteractable)
    {
        _btn.interactable = false;
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonEvent.OnSelectCard)
        {
            OnSelectCard2(photonEvent);
        }
        
    }

    private void OnSelectCard2(EventData photonEvent)
    {
        object[] data = (object[])photonEvent.CustomData;
        int id = (int)data[0];
        TeamID teamID = (TeamID)data[1];
        if (id == this._id)
        {
            BoardManager.instance.AddHero(teamID, _heroID, this);
        }
    }
}

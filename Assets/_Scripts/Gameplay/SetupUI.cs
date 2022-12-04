using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Observer;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class SetupUI : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private TMP_Text _blueFullnameTxt;
    [SerializeField] private TMP_Text _redFullnameTxt;
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void SetUpUI()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameFlowManager.instance.playerTeam = TeamID.Blue;
            _blueFullnameTxt.text = UserManager.instance.fullName;
            SendUIInformation(TeamID.Blue, UserManager.instance.fullName);
        }
        else
        {
            GameFlowManager.instance.playerTeam = TeamID.Red;
            _redFullnameTxt.text = UserManager.instance.fullName;
            SendUIInformation(TeamID.Red, UserManager.instance.fullName);
        }
    }
    
    void OnSetName(EventData photonEvent)
    {
        object[] data = (object[])photonEvent.CustomData;
        TeamID teamID = (TeamID)data[0];
        string name = (string)data[1];
        if (teamID == TeamID.Blue)
        {
            _blueFullnameTxt.text = name;
        }
        else
        {
            _redFullnameTxt.text = name;
        }
    }
    
    private void SendUIInformation(TeamID teamID, string name)
    {
        object[] content = new object[] {teamID, name};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(PhotonEvent.OnSetOpponentName, content, raiseEventOptions, SendOptions.SendReliable);
    }
    
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonEvent.OnGameplayStart)
        {
            SetUpUI();
        }
        if (eventCode == PhotonEvent.OnSetOpponentName)
        {
            OnSetName(photonEvent);
        }
    }
}

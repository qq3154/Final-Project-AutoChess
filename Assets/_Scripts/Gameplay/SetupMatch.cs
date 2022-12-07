using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Observer;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class SetupMatch : MonoBehaviour, IOnEventCallback
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

    private void SetUpMatchInfomation()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameFlowManager.instance.playerTeam = TeamID.Blue;
            _blueFullnameTxt.text = UserManager.instance.fullName;
            SendPlayerInformation(TeamID.Blue, UserManager.instance.fullName, UserManager.instance.username);
        }
        else
        {
            GameFlowManager.instance.playerTeam = TeamID.Red;
            _redFullnameTxt.text = UserManager.instance.fullName;
            SendPlayerInformation(TeamID.Red, UserManager.instance.fullName, UserManager.instance.username);
        }
    }
    
    void OnSetPlayerInfomation(EventData photonEvent)
    {
        object[] data = (object[])photonEvent.CustomData;
        TeamID teamID = (TeamID)data[0];
        string name = (string)data[1];
        string username = (string)data[2];
        
        if (teamID == TeamID.Blue)
        {
            _blueFullnameTxt.text = name;
            MatchManager.instance.userBlue = username;
          
        }
        else
        {
            _redFullnameTxt.text = name;
            MatchManager.instance.userRed = username;
        
        }
    }

    private void SendPlayerInformation(TeamID teamID, string name, string userName)
    {
        object[] content = new object[] {teamID, name, userName};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(PhotonEvent.OnSetOpponentInfomation, content, raiseEventOptions, SendOptions.SendReliable);
    }
    
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonEvent.OnGameplayStart)
        {
            SetUpMatchInfomation();
        }
        if (eventCode == PhotonEvent.OnSetOpponentInfomation)
        {
            OnSetPlayerInfomation(photonEvent);
        }
    }
}

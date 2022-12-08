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

    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMap;
    
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
        GameFlowManager.instance.round = 0;
        GameFlowManager.instance.heroOnBoard = 0;
        
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
        string[] ids = (string[])data[3];
        int[] levels = (int[])data[4];
        
        if (teamID == TeamID.Blue)
        {
            _blueFullnameTxt.text = name;
            MatchManager.instance.userBlue = username;
            
            for (int i = 0; i < ids.Length; i++)
            {
                _heroProfileConfigMap.GetValueFromKey(ids[i]).HeroStats.BlueTeamLevel = levels[i];
            }
        }
        else
        {
            _redFullnameTxt.text = name;
            MatchManager.instance.userRed = username;
            
            for (int i = 0; i < ids.Length; i++)
            {
                _heroProfileConfigMap.GetValueFromKey(ids[i]).HeroStats.RedTeamLevel = levels[i];
            }
        }
    }

    private void SendPlayerInformation(TeamID teamID, string name, string userName)
    {
        List<string> cardIds = new List<string>();
        List<int> cardLevels = new List<int>();
        
        var configs = _heroProfileConfigMap.list;
        for (int i = 0; i < configs.Count; i++)
        {
            cardIds.Add(configs[i].id);
            cardLevels.Add(configs[i].heroConfig.HeroStats.Level);
        }
        string[] arrIds = cardIds.ToArray();
        int[] arrLevels = cardLevels.ToArray();
        
        object[] content = new object[] {teamID, name, userName, arrIds, arrLevels};
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

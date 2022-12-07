using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class SetupOnRoundStart : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private TMP_Text roundTxt;
    [SerializeField] private TMP_Text heroOnBoardTxt;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BoardManager.instance.gameObject != null)
        {
            if (GameFlowManager.instance.playerTeam == TeamID.Blue)
            {
                heroOnBoardTxt.text = BoardManager.instance._onBoardA.Count + "/" + GameFlowManager.instance.heroOnBoard;
            }
        
            if (GameFlowManager.instance.playerTeam == TeamID.Red)
            {
                heroOnBoardTxt.text = BoardManager.instance._onBoardB.Count + "/" + GameFlowManager.instance.heroOnBoard;
            }
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonEvent.OnSelectCardPhaseStart)
        {
            GameFlowManager.instance.round++;
            GameFlowManager.instance.heroOnBoard = Mathf.Min( GameFlowManager.instance.round, GameFlowManager.instance.maxHeroOnBoard);
            roundTxt.text = "Round " + GameFlowManager.instance.round;
            heroOnBoardTxt.text = "5/" + GameFlowManager.instance.heroOnBoard;
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
}

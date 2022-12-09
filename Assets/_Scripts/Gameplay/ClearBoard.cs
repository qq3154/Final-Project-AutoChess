using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearBoard : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private Slider _blueHp;
    [SerializeField] private Slider _redHp;
    [SerializeField] private TMP_Text _blueHpTxt;
    [SerializeField] private TMP_Text _redHpTxt;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonEvent.OnRoundEnd)
        {
            object[] data = (object[])photonEvent.CustomData;
            TeamID teamID = (TeamID)data[0];
            int hp = (int)data[1];
            
            Debug.Log("Round End");

            StartCoroutine(OnEndBattle(teamID, hp));

        }
        
        if (eventCode == PhotonEvent.OnMatchEnd)
        {
            object[] data = (object[])photonEvent.CustomData;
            TeamID teamID = (TeamID)data[0];
            int round = (int)data[1];

            if (teamID == GameFlowManager.instance.playerTeam)
            {
                SendplusGold(UserManager.instance.gold + 200);
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("Win");
                Destroy(BoardManager.instance.gameObject);
            }
            else
            {
                
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("Lose");
                Destroy(BoardManager.instance.gameObject);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
           

        }
        
        if (eventCode == PhotonEvent.OnAFK)
        {
            
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Lose");
            Destroy(BoardManager.instance.gameObject);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
           

        }
    }

    IEnumerator OnEndBattle(TeamID teamID, int hp)
    {
        if (teamID == TeamID.Red)
        {
            _blueHp.DOValue(_blueHp.value - hp, 1).OnComplete(() => _blueHpTxt.text = _blueHp.value.ToString() + "/100"); 
               
        }
            
        if (teamID == TeamID.Blue)
        {
            _redHp.DOValue(_redHp.value - hp, 1).OnComplete(() => _redHpTxt.text = _redHp.value.ToString() + "/100"); 
        }
       
        //Do victory pose
        if (teamID == TeamID.Blue)
        {
            foreach (var hero in  BoardManager.instance._onBoardB)
            {
                hero.gameObject.SetActive(false);
            }
            
            foreach (var hero in  BoardManager.instance._onBoardA)
            {
                hero._axieFigureController.SetOnWin();
            }
        }
        else
        {
            foreach (var hero in  BoardManager.instance._onBoardA)
            {
                hero.gameObject.SetActive(false);
                // Destroy(hero.gameObject);
                // BoardManager.instance._onBoardA.Remove(hero);
            }
            
            foreach (var hero in  BoardManager.instance._onBoardB)
            {
                hero._axieFigureController.SetOnWin();
            }
        }
        

        yield return new WaitForSeconds(2);
        
        
        BoardManager.instance.ClearBoard();

        
        //continue match
        if (_blueHp.value > 0 && _redHp.value > 0)
        {
            BoardManager.instance.SetupBoard();

            if (PhotonNetwork.IsMasterClient)
            {
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(PhotonEvent.OnSelectCardPhaseStart, null, raiseEventOptions, SendOptions.SendReliable);
            }

        }
        else
        {

            TeamID winTeamID = (_blueHp.value <= 0) ? TeamID.Red : TeamID.Blue;
            
            if (PhotonNetwork.IsMasterClient)
            {
                object[] content = new object[] {winTeamID, GameFlowManager.instance.round};
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(PhotonEvent.OnMatchEnd, content, raiseEventOptions, SendOptions.SendReliable);

                string winner = (winTeamID == TeamID.Blue) ? MatchManager.instance.userBlue : MatchManager.instance.userRed;
                string loser = (winTeamID == TeamID.Red) ? MatchManager.instance.userBlue : MatchManager.instance.userRed;
                SendMatchRequest(winner, loser);
            }
            
        }
    }

    private async void SendMatchRequest(string winner, string loser)
    {
        var response = await ApiRequest.instance.SendCreateMatchRequest(winner, loser, GameFlowManager.instance.round);
        
        if (response.success)
        {
            Debug.Log(response);

        }
        else
        {
            Debug.Log(response);
        }
    }
    
    
    private async void SendplusGold(int gold)
    {
        var response = await ApiRequest.instance.SendUpdateGoldRequest(gold);
        if (response.success)
        {
            UserManager.instance.gold = gold;
        }
        else
        {
            Debug.Log(response.message);
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

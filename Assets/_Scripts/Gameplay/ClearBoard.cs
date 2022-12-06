using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
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
                // Destroy(hero.gameObject);
                // BoardManager.instance._onBoardB.Remove(hero);
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
        
        Debug.Log("blue hp =" + _blueHp.value);
        Debug.Log("red hp =" + _redHp.value);

        
        BoardManager.instance.ClearBoard();

        if (_blueHp.value > 0 && _redHp.value > 0)
        {
            BoardManager.instance.SetupBoard();
        }
        
        if (PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(PhotonEvent.OnSelectCardPhaseStart, null, raiseEventOptions, SendOptions.SendReliable);
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

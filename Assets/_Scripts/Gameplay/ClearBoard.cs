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
            
            //Do victory pose
            BoardManager.instance.ClearBoard();

            if (teamID == TeamID.Red)
            {
                _blueHp.DOValue(_blueHp.value - hp, 1).OnComplete(() => _blueHpTxt.text = _blueHp.value.ToString() + "/100"); 
               
            }
            
            if (teamID == TeamID.Blue)
            {
                _redHp.DOValue(_blueHp.value - hp, 1).OnComplete(() => _redHpTxt.text = _redHp.value.ToString() + "/100"); 
            }


            BoardManager.instance.SetupBoard();

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

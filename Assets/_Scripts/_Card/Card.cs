using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Game;
using Observer;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private string _heroID;

    [SerializeField] private Button _btn;
    
    [SerializeField] private AxieSpawner _axieSpawner;

    [SerializeField] private int _id;

    [Header("UI")]
    [SerializeField] private TMP_Text _nameTxt;
    [SerializeField] private TMP_Text _blueLevelTxt;
    [SerializeField] private TMP_Text _redLevelTxt;
    [SerializeField] private List<GameObject> _stars;
    [SerializeField] private Image _specieIcon;
    [SerializeField] private TMP_Text _specieTxt;
    [SerializeField] private Image _classIcon;
    [SerializeField] private TMP_Text _classTxt;
    [SerializeField] private Image _darkBG;
    
    [Header("Ref")]
    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMap;
    [SerializeField] private ClassesIconConfig _classesIconConfig;

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
        SetupUI(heroID);
    }

    private void SetupUI(string heroID)
    {
        HeroStats heroStats = _heroProfileConfigMap.GetValueFromKey(heroID).HeroStats;
        _nameTxt.text = heroStats.Name;
        _blueLevelTxt.text = "Level " + heroStats.BlueTeamLevel.ToString();
        _redLevelTxt.text =  "Level " + heroStats.RedTeamLevel.ToString();
        SetStar(heroStats.Rarity);
        _specieTxt.text = heroStats.Species;
        _classTxt.text = heroStats.Class;
        _specieIcon.sprite = _classesIconConfig.GetIconById(heroStats.Species);
        _classIcon.sprite = _classesIconConfig.GetIconById(heroStats.Class);
    }

    private void SetStar(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _stars[i].SetActive(true);
        }
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
        _darkBG.gameObject.SetActive(true);
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

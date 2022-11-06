using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private string _heroID;

    [SerializeField] private Button _btn;
    
    [SerializeField] private AxieSpawner _axieSpawner;

    private void Awake()
    {
        _btn.onClick.AddListener(() => OnSelectCard(GameFlowManager.instance.playerTeam));
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveAllListeners();
    }

    public void InitCard(string heroID)
    {
        this._heroID = heroID;
        _axieSpawner.Init(heroID);
    }

    public void OnSelectCard(TeamID teamID)
    {
        
        BoardManager.instance.AddHero(teamID, _heroID, this);
        
    }

    public void SetInteractable(bool isInteractable)
    {
        _btn.interactable = false;
    }
}

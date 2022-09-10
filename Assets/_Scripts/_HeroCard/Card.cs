using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private HeroID _heroID;

    [SerializeField] private Button _btn;

    private void Awake()
    {
        _btn.onClick.AddListener(() => OnSelectCard(TeamID.Blue));
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveAllListeners();
    }

    public void InitCard(HeroID heroID)
    {
        this._heroID = heroID;
    }

    public void OnSelectCard(TeamID teamID)
    {
        
        BoardManager.instance.AddHeroToBench(teamID, _heroID, this);
        
    }

    public void SetInteractable(bool isInteractable)
    {
        _btn.interactable = false;
    }
}

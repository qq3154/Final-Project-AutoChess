using System;
using System.Collections;
using System.Collections.Generic;
using Observer;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SelectCardBoard : MonoBehaviour
{
   
    [SerializeField] private GameObject _root;
    [SerializeField] private List<Card> _heroCards;
    [SerializeField] private GameObject _cardHolder;
    
    [SerializeField] private List<HeroID> _heroPool;
    [SerializeField] private int _maxCardToSelect = 5;
    [SerializeField] private GameObject _cardPref;
    private void Awake()
    {
        this.RegisterListener(EventID.OnWaveStart, (param) => DoOnWaveStart());
        this.RegisterListener(EventID.OnSelectCardPhaseStart, (param) => Show());
    }


    private void DoOnWaveStart()
    {
        Hide();
        this.PostEvent(EventID.OnSelectCardPhaseStart);
    }

    private void Hide()
    {
        _root.SetActive(false);
    }

    private void Show()
    {
        _root.SetActive(true);
        InitRandomCard();
    }

    private void InitRandomCard()
    {
       
        
        for (int i = 0; i < _maxCardToSelect; i++)
        {
            int index = Random.Range(0,_heroPool.Count);
            HeroID heroID = _heroPool[index];
            _heroPool.RemoveAt(index);
            
            var instantiate = Instantiate(_cardPref, _cardHolder.transform);

            Card myCard= instantiate.GetComponent<Card>();
            myCard.InitCard(heroID);
        }
    }
}

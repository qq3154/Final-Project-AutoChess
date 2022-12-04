using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Observer;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SelectCardBoard : MonoBehaviour, IOnEventCallback
{
   
    [SerializeField] private GameObject _root;
    [SerializeField] private GameObject _cardHolder;
    [SerializeField] private List<Card> _randomCards;
    
    [SerializeField] private List<string> _heroPool;
    [SerializeField] private int _maxCardToSelect = 5;
    [SerializeField] private GameObject _cardPref;
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Hide()
    {
        foreach (var card in _randomCards)
        {
            Destroy(card.gameObject);
        }

        foreach (Transform child in _cardHolder.transform) {
            Destroy(child.gameObject);
        }
        _root.SetActive(false);
       
    }

    private void Show()
    {
        _root.SetActive(true);
    }

    private void InitRandomCard()
    {
        // _randomCards.Clear();
        List<string> cardIds = new List<string>();
        for (int i = 0; i < _maxCardToSelect; i++)
        {
            int index = Random.Range(0,_heroPool.Count);
            string heroID = _heroPool[index];
            cardIds.Add(heroID);
        }

        string[] arr = cardIds.ToArray();
        object[] content = new object[] {arr};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
        PhotonNetwork.RaiseEvent(PhotonEvent.OnInitCards, content, raiseEventOptions, SendOptions.SendReliable);
    }

    #region Debug function

    public void ShowInDebug()
    {
        if (_root.gameObject.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    #endregion

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonEvent.OnSelectCardPhaseStart)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                InitRandomCard();
            }
            
            
        }

        if (eventCode == PhotonEvent.OnInitCards)
        {
            InitCard(photonEvent);
            Show();
        }
    }

    void InitCard(EventData photonEvent)
    {
        object[] data = (object[])photonEvent.CustomData;
        string[] arr = (string[])data[0];
        List<string> cardIds = new List<string>(arr);
        
        _randomCards.Clear();
        for (int i = 0; i < cardIds.Count; i++)
        {
            var instantiate = Instantiate(_cardPref, _cardHolder.transform);

            Card myCard= instantiate.GetComponent<Card>();
            myCard.InitCard(i, cardIds[i]);
            _randomCards.Add(myCard);
        }

    }
}

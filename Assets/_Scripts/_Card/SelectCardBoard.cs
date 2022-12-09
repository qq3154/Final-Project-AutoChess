using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Observer;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SelectCardBoard : MonoBehaviour, IOnEventCallback
{
   
    [SerializeField] private GameObject _root;
    [SerializeField] private GameObject _cardHolder;
    [SerializeField] private List<Card> _randomCards;

    [SerializeField] private List<HeroPool> _heroPools;
    [SerializeField] private List<string> _heroPool;
    [SerializeField] private int _maxCardToSelect = 5;
    [SerializeField] private GameObject _cardPref;

    [SerializeField] private TMP_Text _cooldown;
    [SerializeField] private Button _openBtn;
    
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
        if (_heroPools.Count <= GameFlowManager.instance.round )
        {
            _heroPool = _heroPools[_heroPools.Count - 1].heroId;
        }
        else
        {
            _heroPool = _heroPools[GameFlowManager.instance.round].heroId;
        }
        
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

            BoardManager.instance._IsLock = false;

            StartCoroutine(IE_Cooldown());

            //cooldown 15s select phase

        }

        if (eventCode == PhotonEvent.OnInitCards)
        {
            foreach (Transform child in _cardHolder.transform) {
                Destroy(child.gameObject);
            }
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

    IEnumerator IE_Cooldown()
    {
        _cooldown.gameObject.SetActive(true);
        for (int i = GameFlowManager.instance.selectPhaseTime; i >= 0; i--)
        {
            _cooldown.SetText(i.ToString());
            yield return new WaitForSeconds(1); 
           
        }
        _root.SetActive(false);
        _openBtn.gameObject.SetActive(false);
        _cooldown.SetText("Fight!");
        BoardManager.instance._IsLock = true;
        yield return new WaitForSeconds(2); 
        _cooldown.SetText("");
        _cooldown.gameObject.SetActive(false);
        
        BoardManager.instance.SaveHeroRecords();
        
        yield return new WaitForSeconds(2); 
        
        BoardManager.instance.CalculateHeroStat();

        if (PhotonNetwork.IsMasterClient)
        {
            BoardManager.instance.StartFight();
        }
    }
}

[Serializable]
public struct HeroPool
{
    public int round;
    public List<string> heroId;

}

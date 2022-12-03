using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardMenu : MonoBehaviour
{
    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMap;
    [SerializeField] private GameObject _scrollviewHolder;
    [SerializeField] private CardMenuItem _cardMenuItemPref;
    [SerializeField] private TMP_InputField _userFullnameTxT;
    [SerializeField] private TMP_Text _userGoldTxt;

    private void Start()
    {
        _userFullnameTxT.text = UserManager.instance.fullName;
        
        foreach (var heroProfile in _heroProfileConfigMap.list)
        {
            var id = heroProfile.id;
            var config = heroProfile.heroConfig;
            var card = Instantiate(_cardMenuItemPref, _scrollviewHolder.transform);
            card.transform.SetParent(_scrollviewHolder.transform);
            card.InitData(id, config);
            card.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        _userGoldTxt.text = UserManager.instance.gold.ToString();
    }
}

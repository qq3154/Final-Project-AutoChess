using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMenu : MonoBehaviour
{
    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMap;
    [SerializeField] private GameObject _scrollviewHolder;
    [SerializeField] private CardMenuItem _cardMenuItemPref;

    private void Start()
    {
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
}

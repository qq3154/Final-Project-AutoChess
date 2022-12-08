using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardMenuItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private List<GameObject> _stars;

    [SerializeField] private AxieSpawner _axieSpawner;
    
    [SerializeField] private Button _upgradeBtn;

    [SerializeField] private string _cardId;
    [SerializeField] private HeroConfig _heroConfig;

    private void Start()
    {
        _upgradeBtn.onClick.AddListener(UpgradeCard);
    }

    private void UpgradeCard()
    {
        SendUpgradeCardRequest(_cardId, 100);
    }

    public async void SendUpgradeCardRequest(string id, int cost)
    {
        var response = await ApiRequest.instance.SendUpgradeCardRequest(id, cost);
        if (response.success)
        {
            _heroConfig.HeroStats.Level += 1; 
            string levelTxt = _heroConfig.HeroStats.Level.ToString() +  "/"  + _heroConfig.HeroStats.MaxLevel.ToString();
            _level.text = levelTxt;
            UserManager.instance.gold -= cost;
            ToastMessage.instance.Show("Upgrade successful!");
        }
        else
        {
            ToastMessage.instance.Show(response.message);
        }
        
    }

    public void InitData(string id, HeroConfig heroConfig)
    {
        _axieSpawner.Init(id);
        _cardId = heroConfig.HeroStats.CardId;
        _heroConfig = heroConfig;

        var rarity = _heroConfig.HeroStats.Rarity;
        SetStar(rarity);
        _name.text = _heroConfig.HeroStats.Name;
        string levelTxt = _heroConfig.HeroStats.Level.ToString() +  "/"  + _heroConfig.HeroStats.MaxLevel.ToString();
        _level.text = levelTxt;
    }

    public void SetStar(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _stars[i].SetActive(true);
        }
    }
}

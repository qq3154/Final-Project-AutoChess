using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardMenuItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _level;

    [SerializeField] private AxieSpawner _axieSpawner;
    
    [SerializeField] private Button _upgradeBtn;

    public void InitData(string id, HeroConfig heroConfig)
    {
        _axieSpawner.Init(id);
        _name.text = heroConfig.HeroStats.Name;
        string levelTxt = heroConfig.HeroStats.Level.ToString() +  "/"  + heroConfig.HeroStats.MaxLevel.ToString();
        _level.text = levelTxt;
    }
}

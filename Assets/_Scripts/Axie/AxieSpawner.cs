using System;
using System.Collections;
using System.Collections.Generic;
using AxieCore.AxieMixer;
using AxieMixer.Unity;
using Newtonsoft.Json.Linq;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class AxieSpawner : MonoBehaviour
{
    [SerializeField] AxieFigureController axieFigureController;
    [SerializeField] private AxieProfile axieProfile;
    [SerializeField] private TeamID teamID;
    
    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMap;
    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMapInMenu;

    [SerializeField] private bool isMenu = false;
    [SerializeField] private string menuId;
    private void Awake()
    {
        Mixer.Init();
    }

    private void Start()
    {
        if (isMenu)
        {
            InitMenu(heroID:menuId);
        }
    }
    
    public void InitMenu(string heroID)
    {
        axieProfile = _heroProfileConfigMapInMenu.GetValueFromKey(heroID).axieProfile;

        axieFigureController.SetGenes(axieProfile, true, isMenu);
    }

    public void Init(string heroID)
    {
        axieProfile = _heroProfileConfigMap.GetValueFromKey(heroID).axieProfile;

        axieFigureController.SetGenes(axieProfile, true, isMenu);
    }

    public void Init(Hero hero)
    {
        axieProfile = _heroProfileConfigMap.GetValueFromKey(hero.HeroID).axieProfile;
        axieFigureController.SetGenes(axieProfile, hero.IsFaceRight, isMenu);
    }

   
    
}

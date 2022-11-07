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
    

    private void Awake()
    {
        Mixer.Init();
    }
    
    public void Init(string heroID)
    {
        //axieId = _heroProfileConfigMap.GetValueFromKey(heroID).AxieId;
        axieProfile = _heroProfileConfigMap.GetValueFromKey(heroID).axieProfile;

        axieFigureController.SetGenes(axieProfile, TeamID.Blue);
    }

    public void Init(Hero hero)
    {
        axieProfile = _heroProfileConfigMap.GetValueFromKey(hero.HeroID).axieProfile;
        axieFigureController.SetGenes(axieProfile, hero.TeamID);
    }

    
}

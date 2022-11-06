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
    
    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMap;

    //bool _isPlaying = false;
    //bool _isFetchingGenes = false;

    private void Awake()
    {
        Mixer.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        //OnMixButtonClicked();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            OnMixButtonClicked();
        }
    }
    
    public void Init(string heroID)
    {
        //axieId = _heroProfileConfigMap.GetValueFromKey(heroID).AxieId;
        axieProfile = _heroProfileConfigMap.GetValueFromKey(heroID).axieProfile;

        OnMixButtonClicked();
    }

    public void Init(Hero hero)
    {
        axieProfile = _heroProfileConfigMap.GetValueFromKey(hero.HeroID).axieProfile;

        OnMixButtonClicked();
    }
    
    

    void OnMixButtonClicked()
    {
        //StartCoroutine(GetAxiesGenes(axieId));
        
        axieFigureController.SetGenes(axieProfile);

        
    }
    
    
}

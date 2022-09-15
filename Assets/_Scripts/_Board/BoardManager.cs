using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity.Examples;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardManager : MonoSingleton<BoardManager>
{
    public int _X;
    public int _Y;

    [FormerlySerializedAs("_heroPositions")] public  List<Hero> _allHeros;
    public  List<Hero> _teamA;
    public  List<Hero> _teamB;
    public  List<Hero> _benchA;
    public  List<Hero> _benchB;
    public List<BenchSlot> _benchASlots;
    public List<BenchSlot> _benchBSlots;
    

    [SerializeField] private GameObject _heroPref;

    public void AddHeroToBench(TeamID teamID, HeroID heroID, Card card)
    {
        List<BenchSlot> slots;
        if (teamID == TeamID.Blue)
        {
            slots = _benchASlots;
        }
        else
        {
            slots = _benchBSlots;
        }
        

        foreach (var slot in slots)
        {
            if (slot.isUse == false)
            {
                var instantiate= Instantiate(_heroPref, slot.transform);
                instantiate.transform.SetParent(slot.transform);
                Hero myHero = instantiate.GetComponent<Hero>();
                myHero.InitHero(teamID, heroID, 1);
                card.SetInteractable(false);
                slot.isUse = true;
                return;
            }
        }

        //noti bench is full
        Debug.Log(teamID + " bench full");
        
       


    }
    
}

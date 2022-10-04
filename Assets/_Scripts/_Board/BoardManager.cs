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
    
    public bool[,] Pos;
    
    public  List<Hero> _allHeros;
    public  List<Hero> _teamA;
    public  List<Hero> _teamB;
    public  List<Hero> _benchA;
    public  List<Hero> _benchB;
    public List<BenchSlot> _benchASlots;
    public List<BenchSlot> _benchBSlots;
    

    [SerializeField] private GameObject _heroPref;

    protected override void DoOnAwake()
    {
        base.DoOnAwake();
        Pos = new bool[_X, _Y];
        for (int i = 0; i < _X; i++)
        {
            for (int j = 0; j < _Y; j++)
            {
                Pos[i, j] = false;
            }
        }
        foreach (var hero in _allHeros)
        {
            Pos[hero.PosX, hero.PosY] = true;
        }
    }

    public void AddHeroToBench(TeamID teamID, HeroID heroID, Card card)
    {
        List<BenchSlot> slots;
        List<Hero> teams;
        List<Hero> benchs;
        
        if (teamID == TeamID.Blue)
        {
            slots = _benchASlots;
            teams = _teamA;
            benchs = _benchA;
        }
        else
        {
            slots = _benchBSlots;
            teams = _teamB;
            benchs = _benchB;
        }
        
        
        

        List<Hero> oneStarHero = new List<Hero>();
        List<Hero> twoStarHero = new List<Hero>();

        foreach (var hero in teams)
        {
            if (hero.HeroID == heroID)
            {
                if (hero.Level == 1)
                {
                    oneStarHero.Add(hero);
                }
                if (hero.Level == 2)
                {
                    twoStarHero.Add(hero);
                }
            }
            
        }
        
        foreach (var hero in benchs)
        {
            if (hero.HeroID == heroID)
            {
                if (hero.Level == 1)
                {
                    oneStarHero.Add(hero);
                }
                if (hero.Level == 2)
                {
                    twoStarHero.Add(hero);
                }
            }
        }

        //update 3 star
        if (oneStarHero.Count == 2 && twoStarHero.Count == 2)
        {
            //find a 2star hero on board
            //find a 1 star hero on board
            //find first 2star hero on bench
            //upgrade that hero and destroy others
            
            //return 
            
            Debug.Log("upgrade 3 star");
        }
        
        //update 2 star
        if (oneStarHero.Count == 2)
        {

            //find a 1 star hero on board
            foreach (var hero in oneStarHero)
            {
                if (teams.Contains(hero))
                {
                    hero.LevelUp();

                    oneStarHero.Remove(hero);

                    foreach (var otherhero in oneStarHero)
                    {
                        Destroy(otherhero.gameObject);
                    }

                    return;
                }
            }
            
            
            //find first 1 star hero on bench
            foreach (var hero in oneStarHero)
            {
                if (benchs.Contains(hero))
                {
                    hero.LevelUp();

                    oneStarHero.Remove(hero);

                    foreach (var otherhero in oneStarHero)
                    {
                        Destroy(otherhero.gameObject);
                    }

                    return;
                }
            }
            
            //upgrade that hero and destroy 1 1star hero
            Debug.Log("upgrade 2 star");
            return;



        }

        
        //add to bench
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
                benchs.Add(myHero);
                return;
            }
        }

        //noti bench is full
        Debug.Log(teamID + " bench full");
        
       


    }
    
}

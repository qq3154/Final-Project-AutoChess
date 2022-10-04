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
    
    public List<Hero> _allHeros;
    public List<Hero> _onBoardA;
    public List<Hero> _onBoardB;
    public List<Hero> _benchA;
    public List<Hero> _benchB;
    public List<BenchSlot> _benchSlotA;
    public List<BenchSlot> _benchSlotB;
    

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

    public void AddHero(TeamID teamID, HeroID heroID, Card card)
    {
        List<BenchSlot> benchSlots;
        List<Hero> heroOnBoards;
        List<Hero> heroOnBenchs;
        
        if (teamID == TeamID.Blue)
        {
            benchSlots = _benchSlotA;
            heroOnBoards = _onBoardA;
            heroOnBenchs = _benchA;
        }
        else
        {
            benchSlots = _benchSlotB;
            heroOnBoards = _onBoardB;
            heroOnBenchs = _benchB;
        }
        
        
        

        List<Hero> oneStarHeros = new List<Hero>();
        List<Hero> twoStarHeros = new List<Hero>();

        foreach (var hero in heroOnBoards)
        {
            if (hero.HeroID == heroID)
            {
                if (hero.Level == 1)
                {
                    oneStarHeros.Add(hero);
                }
                if (hero.Level == 2)
                {
                    twoStarHeros.Add(hero);
                }
            }
            
        }
        
        foreach (var hero in heroOnBenchs)
        {
            if (hero.HeroID == heroID)
            {
                if (hero.Level == 1)
                {
                    oneStarHeros.Add(hero);
                }
                if (hero.Level == 2)
                {
                    twoStarHeros.Add(hero);
                }
            }
        }

        // //update 3 star
        // if (oneStarHero.Count == 2 && twoStarHero.Count == 2)
        // {
        //     card.SetInteractable(false);
        //     
        //     //find a 2star hero on board
        //     //find a 1 star hero on board
        //     //find first 2star hero on bench
        //     //upgrade that hero and destroy others
        //     
        //     //return 
        //     
        //     Debug.Log("upgrade 3 star");
        // }
        
        //update 2 star
        if (oneStarHeros.Count == 2)
        {
            card.SetInteractable(false);
            
            //find a 1 star hero on board
            foreach (var hero in oneStarHeros)
            {
                if (heroOnBoards.Contains(hero))
                {
                    hero.LevelUp();

                    oneStarHeros.Remove(hero);

                    foreach (var otherhero in oneStarHeros)
                    {
                        RemoveHero(otherhero, benchSlots, heroOnBoards, heroOnBenchs);
                    }

                    return;
                }
            }
            
            
            //find first 1 star hero on bench
            foreach (var hero in oneStarHeros)
            {
                if (heroOnBenchs.Contains(hero))
                {
                    hero.LevelUp();

                    oneStarHeros.Remove(hero);

                    foreach (var otherhero in oneStarHeros)
                    {
                        RemoveHero(otherhero, benchSlots, heroOnBoards, heroOnBenchs);
                    }

                    return;
                }
            }
            
            //upgrade that hero and destroy 1 1star hero
            Debug.Log("upgrade 2 star");
            return;



        }

        
        //add to bench
        foreach (var slot in benchSlots)
        {
            if (slot.isUse == false)
            {
                var instantiate= Instantiate(_heroPref, slot.transform);
                instantiate.transform.SetParent(slot.transform);
                Hero myHero = instantiate.GetComponent<Hero>();
                myHero.InitHero(teamID, heroID, 1);
                card.SetInteractable(false);
                slot.isUse = true;
                slot.SetHero(myHero);
                heroOnBenchs.Add(myHero);
                return;
            }
        }

        //noti bench is full
        Debug.Log(teamID + " bench full");
        
       


    }

    void RemoveHero(Hero hero, List<BenchSlot> benchSlots, List<Hero> heroOnBoards, List<Hero> heroOnBenchs)
    {
        foreach (var bench in benchSlots)
        {
            if (bench.GetHero() == hero)
            {
                bench.SetHero(null);
                bench.isUse = false;
            }
        }

        if (heroOnBoards.Contains(hero))
        {
            heroOnBoards.Remove(hero);
        }
        
        if (heroOnBenchs.Contains(hero))
        {
            heroOnBenchs.Remove(hero);
        }

        _allHeros.Remove(hero);
        
        Destroy(hero.gameObject);
    }
    
}

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

    public GameObject _fightBoardRoot;

    public Hero _currentSelectA;
    public Hero _currentSelectB;
    

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

    public void AddHero(TeamID teamID, string heroID, Card card)
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
        
        foreach (var benchSlot in benchSlots)
        {
            if (benchSlot.isUse)
            {
                var hero = benchSlot.GetHero();
                if (hero != null)
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
            }
            
            
        }
        
        //update 3 star
        if (oneStarHeros.Count == 2 && twoStarHeros.Count == 2)
        {
            card.SetInteractable(false);
            
            //find a 2star hero on board
            foreach (var hero in twoStarHeros)
            {
                if (heroOnBoards.Contains(hero))
                {
                    hero.LevelUp();

                    twoStarHeros.Remove(hero);
                    
                    foreach (var otherhero in twoStarHeros)
                    {
                        RemoveHero(otherhero, benchSlots, heroOnBoards, heroOnBenchs);
                    }

                    foreach (var otherhero in oneStarHeros)
                    {
                        RemoveHero(otherhero, benchSlots, heroOnBoards, heroOnBenchs);
                    }

                    return;
                }
            }
            
            //find a 1star hero on board
            foreach (var hero in oneStarHeros)
            {
                if (heroOnBoards.Contains(hero))
                {
                    hero.LevelUp();

                    oneStarHeros.Remove(hero);
                    
                    foreach (var otherhero in twoStarHeros)
                    {
                        RemoveHero(otherhero, benchSlots, heroOnBoards, heroOnBenchs);
                    }

                    foreach (var otherhero in oneStarHeros)
                    {
                        RemoveHero(otherhero, benchSlots, heroOnBoards, heroOnBenchs);
                    }

                    return;
                    
                    
                }
            }
            
            //find a 2star hero on bench
            foreach (var hero in twoStarHeros)
            {
                if (heroOnBenchs.Contains(hero))
                {
                    hero.LevelUp();

                    twoStarHeros.Remove(hero);
                    
                    foreach (var otherhero in twoStarHeros)
                    {
                        RemoveHero(otherhero, benchSlots, heroOnBoards, heroOnBenchs);
                    }

                    foreach (var otherhero in oneStarHeros)
                    {
                        RemoveHero(otherhero, benchSlots, heroOnBoards, heroOnBenchs);
                    }

                    return;
                }
            }
            
            Debug.Log("upgrade 2 star");
            return;
            
        }
        
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
                bench.RemoveHero();
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

    void MoveHeroToBoard(TeamID teamID, Hero hero, int x, int y)
    {
        if (teamID == TeamID.Blue)
        {
            hero.transform.SetParent(_fightBoardRoot.transform);
            hero.transform.position = new Vector2(x, y);
            hero.PosX = x;
            hero.PosY = y;

            foreach (var benchSlot in _benchSlotA)
            {
                if (benchSlot.isUse)
                {
                    if (benchSlot.GetHero() == hero)
                    {
                        benchSlot.RemoveHero();
                    }
                }
            }
            
            if (_benchA.Contains(hero))
            {
                _benchA.Remove(hero);
            }
            
            if (!_onBoardA.Contains(hero))
            {
                _onBoardA.Add(hero);
            }
          
            _currentSelectA._heroVFXController.SetSelectVFXEnable(false);
            _currentSelectA = null;
        }
    }
    
    void MoveHeroToBench(TeamID teamID, Hero hero, int index)
    {
        if (teamID == TeamID.Blue)
        {
            // hero.transform.SetParent(_fightBoardRoot.transform);
            // hero.transform.position = new Vector2(x, y);
            // hero.PosX = x;
            // hero.PosY = y;
            
            hero.transform.SetParent( _benchSlotA[index].transform);
            hero.transform.localPosition = Vector2.zero;
            
            _benchSlotA[index].SetHero(hero);
            
            if (!_benchA.Contains(hero))
            {
                _benchA.Add(hero);
            }
            
            if (_onBoardA.Contains(hero))
            {
                _onBoardA.Remove(hero);
            }
            
        
            _currentSelectA._heroVFXController.SetSelectVFXEnable(false);
            _currentSelectA = null;
        }
    }

    public void SelectHero(Hero hero)
    {
        if (hero.TeamID == TeamID.Blue)
        {
            if (_currentSelectA != null)
            {
                if (_currentSelectA == hero)
                {
                    _currentSelectA = null;
                    hero._heroVFXController.SetSelectVFXEnable(false);
                    return;
                }
                _currentSelectA._heroVFXController.SetSelectVFXEnable(false);
            } 
            _currentSelectA = hero;
            _currentSelectA._heroVFXController.SetSelectVFXEnable(true);
        }
        else
        {
            _currentSelectB = hero;
        }
    }
    

    public void SelectCell(TeamID teamID, int x, int y)
    {
        if (teamID == TeamID.Blue)
        {
            if (_currentSelectA != null)
            {
                MoveHeroToBoard(teamID, _currentSelectA, x, y);
            }
        }
    }
    
    public void SelectBench(TeamID teamID, int index)
    {
        if (teamID == TeamID.Blue)
        {
            if (_currentSelectA != null)
            {
                MoveHeroToBench(teamID, _currentSelectA, index);
            }
        }
    }

    public void StartFight()
    {
        foreach (var hero in _allHeros)
        {
            hero._heroBT.enabled = true;
        }
    }
    
}

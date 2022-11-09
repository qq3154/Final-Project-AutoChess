using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Observer;
using Spine.Unity.Examples;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardManager : MonoSingleton<BoardManager>
{
    #region Field
    public int _X;
    public int _Y;
    
    public bool[,] Pos;
    
    //public List<Hero> _allHeros;
    public List<Hero> _onBoardA;
    public List<Hero> _onBoardB;
    public List<Hero> _benchA;
    public List<Hero> _benchB;
    public List<BenchSlot> _benchSlotA;
    public List<BenchSlot> _benchSlotB;

    public Dictionary<string, int> _strategiesA = new Dictionary<string, int>();
    public Dictionary<string, int> _strategiesB = new Dictionary<string, int>();

    public Hero _currentSelectA;
    public Hero _currentSelectB;
    
    public GameObject _fightBoardRoot;
    [SerializeField] private GameObject _heroPref;
    
    #endregion

    #region Unity function
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
    }
    
    #endregion

    #region Board function
    public void StartFight()
    {
        foreach (var hero in AllHeroes())
        {
            hero._heroBT.enabled = true;
        }
    }
    
    #endregion

    #region Hero function

    public void AddHero(TeamID teamID, string heroID, Card card)
    {
        List<BenchSlot> benchSlots = PlayerBenchSlot();
        List<Hero> heroOnBoards = PlayerOnBoard();
        List<Hero> heroOnBenchs = PlayerBench();

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
                myHero.name = myHero.HeroStats.Name;
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

        //_allHeros.Remove(hero);
        
        Destroy(hero.gameObject);
    }

    void MoveHeroToBoard(TeamID teamID, Hero hero, int x, int y)
    {
        hero.transform.SetParent(_fightBoardRoot.transform);
        hero.transform.position = new Vector2(x, y);
        hero.PosX = x;
        hero.PosY = y;

        foreach (var benchSlot in PlayerBenchSlot())
        {
            if (benchSlot.isUse)
            {
                if (benchSlot.GetHero() == hero)
                {
                    benchSlot.RemoveHero();
                }
            }
        }
        
        if (PlayerBench().Contains(hero))
        {
            PlayerBench().Remove(hero);
        }
        
        if (!PlayerOnBoard().Contains(hero))
        {
            PlayerOnBoard().Add(hero);
        }
      
        PlayerCurrentSelect()._heroVFXController.SetSelectVFXEnable(false);
        SetPlayerCurrentSelect(null);
        
        this.PostEvent(EventID.OnAddHeroToBoard, hero);
    }
    
    void MoveHeroToBench(TeamID teamID, Hero hero, int index)
    {
        hero.transform.SetParent( PlayerBenchSlot()[index].transform);
        hero.transform.localPosition = Vector2.zero;
        
        PlayerBenchSlot()[index].SetHero(hero);
        
        if (!PlayerBench().Contains(hero))
        {
            PlayerBench().Add(hero);
        }
        
        if (PlayerOnBoard().Contains(hero))
        {
            PlayerOnBoard().Remove(hero);
        }
        
        PlayerCurrentSelect()._heroVFXController.SetSelectVFXEnable(false);
        SetPlayerCurrentSelect(null);
        
        this.PostEvent(EventID.OnRemoveHeroFromBoard, hero);
    }

    #endregion
    
    #region User Input
    public void SelectHero(Hero hero)
    {
        if (PlayerCurrentSelect() != null)
        {
            if (PlayerCurrentSelect() == hero)
            {
                SetPlayerCurrentSelect(null);
                hero._heroVFXController.SetSelectVFXEnable(false);
                return;
            }
            PlayerCurrentSelect()._heroVFXController.SetSelectVFXEnable(false);
        } 
        SetPlayerCurrentSelect(hero);
        PlayerCurrentSelect()._heroVFXController.SetSelectVFXEnable(true);
        
    }

    public void SelectCell(TeamID teamID, int x, int y)
    {
      
        if (PlayerCurrentSelect() != null)
        {
            MoveHeroToBoard(teamID, PlayerCurrentSelect(), x, y);
        }
    }
    
    public void SelectBench(TeamID teamID, int index)
    {
        if (PlayerCurrentSelect() != null)
        {
            MoveHeroToBench(teamID, PlayerCurrentSelect(), index);
        }
    }


    #endregion

    #region Get field shortcut

    public List<Hero> AllHeroes()
    {
        List<Hero> ans = new List<Hero>();  
       
        
         ans = _onBoardA.Concat(_onBoardB).ToList();
         return ans;
    }
    
    public List<Hero> PlayerOnBoard()
    {
        return (GameFlowManager.instance.playerTeam == TeamID.Blue) ? _onBoardA : _onBoardB;
    }
    
    public List<Hero> PlayerBench()
    {
        return (GameFlowManager.instance.playerTeam == TeamID.Blue) ? _benchA : _benchB;
    }
    
    public List<BenchSlot> PlayerBenchSlot()
    {
        return (GameFlowManager.instance.playerTeam == TeamID.Blue) ? _benchSlotA : _benchSlotB;
    }

    public Dictionary<string, int> PlayerStrategies()
    {
        return (GameFlowManager.instance.playerTeam == TeamID.Blue) ? _strategiesA : _strategiesB;
    }

    public Hero PlayerCurrentSelect()
    {
        return (GameFlowManager.instance.playerTeam == TeamID.Blue) ? _currentSelectA : _currentSelectB;
    }

    public void SetPlayerCurrentSelect(Hero hero)
    {
        if (GameFlowManager.instance.playerTeam == TeamID.Blue)
        {
            _currentSelectA = hero;
        }
        else
        {
            _currentSelectB = hero;
        }
    }
    

    #endregion
}

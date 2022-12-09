using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using JetBrains.Annotations;
using Observer;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Spine.Unity.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardManager : MonoSingleton<BoardManager>, IOnEventCallback
{
    protected override bool IsDontDestroyThisObject { get; }

    #region Field
    public int _X;
    public int _Y;

    public bool _IsLock;
    public bool _IsEnd;
    
    public bool[,] Pos;
    
    //public List<Hero> _allHeros;
    public List<Hero> _onBoardA;
    public List<Hero> _onBoardB;
    public List<Hero> _benchA;
    public List<Hero> _benchB;
    public List<BenchSlot> _benchSlotA;
    public List<BenchSlot> _benchSlotB;
    
    public List<HeroRecord> _HeroRecords = new List<HeroRecord>();

    public Dictionary<string, int> _strategiesA = new Dictionary<string, int>();
    public Dictionary<string, int> _strategiesB = new Dictionary<string, int>();

    public Hero _currentSelectA;
    public Hero _currentSelectB;
    
    public GameObject _fightBoardRoot;
    [SerializeField] private GameObject _heroPref;
    
    #endregion
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

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

    public void CalculateHeroStat()
    {
        foreach (var hero in AllHeroes())
        {
            TeamID teamID = hero.TeamID;
            int cardLevel = (teamID == TeamID.Blue) ? hero.HeroStats.BlueTeamLevel : hero.HeroStats.RedTeamLevel;

            //card bonus
            hero.HeroStats.Hp = hero.HeroStats.Hp + (cardLevel - 1) * 10;
            hero.HeroStats.Dmg = hero.HeroStats.Dmg + (cardLevel - 1) * 2;
            hero.HeroStats.SkillDmg = hero.HeroStats.SkillDmg + (cardLevel - 1) * 3;

            //level bonus
            if (hero.Level == 2)
            {
                hero.HeroStats.Hp = hero.HeroStats.Hp * 1.8f;
                hero.HeroStats.Dmg = hero.HeroStats.Dmg * 1.8f;
                hero.HeroStats.SkillDmg = hero.HeroStats.SkillDmg * 1.5f;
            }
            
            if (hero.Level == 3)
            {
                hero.HeroStats.Hp = hero.HeroStats.Hp * 2.5f;
                hero.HeroStats.Dmg = hero.HeroStats.Dmg * 2.5f;
                hero.HeroStats.SkillDmg = hero.HeroStats.SkillDmg * 2f;
            }

            if (PlayerStrategies(teamID).ContainsKey(hero.HeroStats.Class))
            {
                var classCount = PlayerStrategies(teamID)[hero.HeroStats.Class];
                switch (hero.HeroStats.Class)
                {
                    case "Warrior":
                        hero.HeroStats.Hp += classCount * 50;
                        break;
                    case "Knight":
                        hero.HeroStats.Hp += classCount * (hero.HeroStats.Hp / 20);
                        break;
                    case "Mage":
                        hero.HeroStats.MaxMana -= classCount * 10;
                        break;
                    case "Hunter":
                        hero.HeroStats.Dmg += classCount * 20;
                        break;
                    default:
                        Debug.LogWarning("Not Found Class" + hero.HeroStats.Class);
                        break;
                }
            } 
            
            if (PlayerStrategies(teamID).ContainsKey(hero.HeroStats.Species))
            {
                var speciesCount = PlayerStrategies(teamID)[hero.HeroStats.Species];
                switch (hero.HeroStats.Species)
                {
                    case "Dragon":
                        hero.HeroStats.Dmg += speciesCount * 25;
                        break;
                    case "Elf":
                        hero.HeroStats.SkillDmg += speciesCount * 50;
                        break;
                    case "Beast":
                        hero.HeroStats.AtkSpeed += speciesCount * 0.3f;
                        break;
                    case "Naga":
                        hero.HeroStats.Hp += speciesCount * 70;
                        break;
                    default:
                        Debug.LogWarning("Not Found Species" + hero.HeroStats.Species);
                        break;
                }
            } 
            
            
            //set init HUD
            hero._heroHUD.SetSliderInitValue( hero.HeroStats.Hp , hero.HeroStats.MaxMana);
        }
    }
    
    
    public void StartFight()
    {
        _IsEnd = false;
        for (int i = 0; i < _X; i++)
        {
            for (int j = 0; j < _Y; j++)
            {
                Pos[i, j] = false;
            }
        }

        //2 player AFK
        if (AllHeroes().Count == 0)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(PhotonEvent.OnAFK, null, raiseEventOptions, SendOptions.SendReliable);
            return;
        }

        if (PlayerOnBoard(TeamID.Blue).Count == 0)
        {
            object[] content = new object[] { TeamID.Red , BoardManager.instance._onBoardB.Count * GameFlowManager.instance.hpLosePerHero}; 
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };  
            PhotonNetwork.RaiseEvent(PhotonEvent.OnRoundEnd, content, raiseEventOptions, SendOptions.SendReliable);
            return;
        }
        
        if (PlayerOnBoard(TeamID.Red).Count == 0)
        {
            object[] content = new object[] { TeamID.Blue , BoardManager.instance._onBoardA.Count * GameFlowManager.instance.hpLosePerHero}; 
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };  
            PhotonNetwork.RaiseEvent(PhotonEvent.OnRoundEnd, content, raiseEventOptions, SendOptions.SendReliable);
            return;
        }
        
        foreach (var hero in AllHeroes())
        {
            Pos[hero.PosX, hero.PosY] = true;
            hero._heroBT.enabled = true;
        }
    }

    public void SaveHeroRecords()
    {
        _HeroRecords.Clear();
        foreach (var hero in AllHeroes())
        {
            HeroRecord heroRecord = new HeroRecord();
            heroRecord.teamID = hero.TeamID;
            heroRecord.heroID = hero.HeroID;
            heroRecord.level = hero.Level;
            heroRecord.posX = hero.PosX;
            heroRecord.posY = hero.PosY;
            _HeroRecords.Add(heroRecord);
            
            
            
        }
    }

    public void SetupBoard()
    {
        foreach (var heroRecord in _HeroRecords)
        {
            var instantiate= Instantiate(_heroPref);
            instantiate.transform.SetParent(_fightBoardRoot.transform);
            Hero hero = instantiate.GetComponent<Hero>();
            hero.InitHero(heroRecord.teamID, heroRecord.heroID, heroRecord.level);
            hero.name = hero.HeroStats.Name;
            
            hero.transform.position = new Vector2(heroRecord.posX, heroRecord.posY);
            hero.PosX = heroRecord.posX;
            hero.PosY = heroRecord.posY;
            
        
            if (!PlayerOnBoard(heroRecord.teamID).Contains(hero))
            {
                PlayerOnBoard(heroRecord.teamID).Add(hero);
            }
        }
        
    }

    public void ClearBoard()
    {
        foreach (var hero in AllHeroes())
        {
            if (hero.TeamID == TeamID.Blue)
            {  
                BoardManager.instance._onBoardA.Remove(hero);
            }
            else
            {
                BoardManager.instance._onBoardB.Remove(hero);
            }
            
            BoardManager.instance.Pos[hero.PosX, hero.PosY] = false;
            AllHeroes().Remove(hero);
            Destroy(hero.gameObject);
            
        }
    }

    [Serializable]
    public struct HeroRecord
    {
        public TeamID teamID;
        public string heroID;
        public int level;
        public int posX;
        public int posY;
    }

    #endregion

    #region Hero function
    
    

    public void AddHero(TeamID teamID, string heroID, Card card)
    {
        List<BenchSlot> benchSlots = PlayerBenchSlot(teamID);
        List<Hero> heroOnBoards = PlayerOnBoard(teamID);
        List<Hero> heroOnBenchs = PlayerBench(teamID);

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
        
        int i = 0;
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
                if (teamID == TeamID.Blue)
                {
                    myHero.PosX = -1;
                    myHero.PosY = i;
                }
                else
                {
                    myHero.PosX = -2;
                    myHero.PosY = i;
                }
                card.SetInteractable(false);
                slot.SetHero(myHero);
                heroOnBenchs.Add(myHero);
                return;
            }
            i++;
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
        if (PlayerOnBoard(teamID).Count == GameFlowManager.instance.heroOnBoard &&
            !PlayerOnBoard(teamID).Contains(hero))
        {
            Debug.Log("Board full");
            return;
        }
        
        hero.transform.SetParent(_fightBoardRoot.transform);
        hero.transform.position = new Vector2(x, y);
        hero.PosX = x;
        hero.PosY = y;

        foreach (var benchSlot in PlayerBenchSlot(teamID))
        {
            if (benchSlot.isUse)
            {
                if (benchSlot.GetHero() == hero)
                {
                    benchSlot.RemoveHero();
                }
            }
        }
        
        if (PlayerBench(teamID).Contains(hero))
        {
            PlayerBench(teamID).Remove(hero);
        }
        
        if (!PlayerOnBoard(teamID).Contains(hero))
        {
            PlayerOnBoard(teamID).Add(hero);
        }
        
        if (teamID == GameFlowManager.instance.playerTeam)
        {
            PlayerCurrentSelect()._heroVFXController.SetSelectVFXEnable(false);
            SetPlayerCurrentSelect(null);
        }
        
        this.PostEvent(EventID.OnAddHeroToBoard, hero);
    }
    
    void MoveHeroToBench(TeamID teamID, Hero hero, int index)
    {
        foreach (var benchSlot in PlayerBenchSlot(teamID))
        {
            if (benchSlot.GetHero() == hero)
            {
                benchSlot.RemoveHero();
            }
        }
        hero.transform.SetParent( PlayerBenchSlot(teamID)[index].transform);
        hero.transform.localPosition = Vector2.zero;
        
        PlayerBenchSlot(teamID)[index].SetHero(hero);
        
        if (!PlayerBench(teamID).Contains(hero))
        {
            PlayerBench(teamID).Add(hero);
        }
        
        if (PlayerOnBoard(teamID).Contains(hero))
        {
            PlayerOnBoard(teamID).Remove(hero);
        }

        if (teamID == TeamID.Blue)
        {
            hero.PosX = -1;
            hero.PosY = index;
        }
        else
        {
            hero.PosX = -2;
            hero.PosY = index;
        }

        if (teamID == GameFlowManager.instance.playerTeam)
        {
            PlayerCurrentSelect()._heroVFXController.SetSelectVFXEnable(false);
            SetPlayerCurrentSelect(null);
        }
       
        
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
            object[] content = new object[] {teamID, x, y, PlayerCurrentSelect().PosX, PlayerCurrentSelect().PosY}; 
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, };  
            PhotonNetwork.RaiseEvent(PhotonEvent.OnMoveHeroToBoard, content, raiseEventOptions, SendOptions.SendReliable);
            //MoveHeroToBoard(teamID, PlayerCurrentSelect(), x, y);
        }
    }
    
    public void SelectBench(TeamID teamID, int index)
    {
        if (PlayerCurrentSelect() != null)
        {
            object[] content = new object[] {teamID, index, PlayerCurrentSelect().PosX, PlayerCurrentSelect().PosY}; 
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };  
            PhotonNetwork.RaiseEvent(PhotonEvent.OnMoveHeroToBench, content, raiseEventOptions, SendOptions.SendReliable);
            //MoveHeroToBench(teamID, PlayerCurrentSelect(), index);
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

    public Hero GetHeroByPosition(int x, int y)
    {
        if (x == -1)
        {
            return _benchSlotA[y].GetHero();
        }
        if (x == -2)
        {
            return _benchSlotB[y].GetHero();
        }

        foreach (var hero in AllHeroes())
        {
            if (hero.PosX == x && hero.PosY == y)
            {
                return hero;
            }
        }

        return null;
    }
    
    public List<Hero> PlayerOnBoard()
    {
        return (GameFlowManager.instance.playerTeam == TeamID.Blue) ? _onBoardA : _onBoardB;
    }
    
    public List<Hero> PlayerOnBoard(TeamID teamID)
    {
        return (teamID == TeamID.Blue) ? _onBoardA : _onBoardB;
    }
    
    public List<Hero> PlayerBench()
    {
        return (GameFlowManager.instance.playerTeam == TeamID.Blue) ? _benchA : _benchB;
    }
    public List<Hero> PlayerBench(TeamID teamID)
    {
        return (teamID == TeamID.Blue) ? _benchA : _benchB;
    }
    
    public List<BenchSlot> PlayerBenchSlot()
    {
        return (GameFlowManager.instance.playerTeam == TeamID.Blue) ? _benchSlotA : _benchSlotB;
    }
    public List<BenchSlot> PlayerBenchSlot(TeamID teamID)
    {
        return (teamID == TeamID.Blue) ? _benchSlotA : _benchSlotB;
    }
    

    public Dictionary<string, int> PlayerStrategies()
    {
        return (GameFlowManager.instance.playerTeam == TeamID.Blue) ? _strategiesA : _strategiesB;
    }
    public Dictionary<string, int> PlayerStrategies(TeamID teamID)
    {
        return (teamID == TeamID.Blue) ? _strategiesA : _strategiesB;
    }

    public Hero PlayerCurrentSelect()
    {
        return (GameFlowManager.instance.playerTeam == TeamID.Blue) ? _currentSelectA : _currentSelectB;
    }
    public Hero PlayerCurrentSelect(TeamID teamID)
    {
        return (teamID == TeamID.Blue) ? _currentSelectA : _currentSelectB;
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

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonEvent.OnMoveHeroToBench)
        {
            object[] data = (object[])photonEvent.CustomData;
            TeamID teamId = (TeamID)data[0];
            int index = (int)data[1];
            int posX = (int)data[2];
            int posY = (int)data[3];

            Hero hero  = GetHeroByPosition(posX, posY);
            if (!hero)
            {
               //todo
               //hanlde missing event
               Debug.LogError("move hero to bench fail");
               return;
            }
            
            MoveHeroToBench(teamId, hero, index);
            
            Debug.Log("on move hero to bench");
        }
        
        if (eventCode == PhotonEvent.OnMoveHeroToBoard)
        {
            object[] data = (object[])photonEvent.CustomData;
            TeamID teamId = (TeamID)data[0];
            int indexX = (int)data[1];
            int indexY = (int)data[2];
            int posX = (int)data[3];
            int posY = (int)data[4];

            Hero hero  = GetHeroByPosition(posX, posY);
            if (!hero)
            {
                //todo
                //hanlde missing event
                Debug.LogError("move hero to board fail");
                return;
            }
            
            MoveHeroToBoard(teamId, hero, indexX, indexY);
            
            Debug.Log("on move hero to bench");
        }
        
        
    }
}

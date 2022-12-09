using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hero : MonoBehaviour
{
    [Header("ID")]
    [SerializeField] public string HeroID;
    [SerializeField] public int Level;
    [SerializeField] public TeamID TeamID; 
    
    [Header("Position")]
    [SerializeField] public int PosX;  
    [SerializeField] public int PosY;
    [SerializeField] public bool IsFaceRight;
    [SerializeField] public int CurrentMana;
    
    [Header("Target")]
    [SerializeField] public Hero Target;
    [SerializeField] public int TargetPosX;
    [SerializeField] public int TargetPosY;
    
    [Header("Stats")]
    [SerializeField] public HeroStats HeroStats;

    [Header("Axie")]
    [SerializeField] private AxieSpawner _axieSpawner;
    [SerializeField] public AxieFigureController _axieFigureController;
    [SerializeField] public HeroHUD _heroHUD;
    [SerializeField] public HeroVFXController _heroVFXController;
    [SerializeField] public HeroBT _heroBT;
    
    [Header("Profile Config")]
    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMap;
    
    [Header("Bullet pref")]
    [SerializeField] public RangeAttackBullet _rangeAttackBulletPref;
    [SerializeField] public GameObject _bulletRoot;
    
    public void InitHero(TeamID teamID, string heroID, int level)
    {
        TeamID = teamID;
        HeroID = heroID;
        Level = level;
        IsFaceRight = teamID == TeamID.Blue; 
        _axieSpawner.Init(this);
        _heroHUD.SetLevel(Level);
        _heroHUD.SetSliderBar(teamID);

        var config = _heroProfileConfigMap.GetValueFromKey(heroID);

        HeroStats = config.HeroStats;
    }

    public void LevelUp()
    {
        Level++;
        _heroHUD.SetLevel(Level);
        _heroVFXController.PlayLevelUpVFX();
    }


    public void OnDamage(float dmg)
    {
        HeroStats.Hp -= dmg;
        if (HeroStats.Hp  <= 0)
        {
            Dead();
        }

        CurrentMana += 10;
        
        _heroHUD.SetHpValue(HeroStats.Hp);
        _heroHUD.SetManaValue(CurrentMana);
        
    }
    
    private void Dead()
    {
        BoardManager.instance.AllHeroes().Remove(this);
        if (this.TeamID == TeamID.Blue)
        {  
            BoardManager.instance._onBoardA.Remove(this);
        }
        else
        {
            BoardManager.instance._onBoardB.Remove(this);
        }

        BoardManager.instance.Pos[PosX, PosY] = false;
        
        Destroy(this.gameObject);


        if (PhotonNetwork.IsMasterClient)
        {
            if (!BoardManager.instance._IsEnd)
            {
                if ( BoardManager.instance._onBoardA.Count == 0)
                {
                    BoardManager.instance._IsEnd = true;
                    Debug.Log("Red win round");
                
                    foreach (var hero in BoardManager.instance.AllHeroes())
                    {
                        hero._heroBT.enabled = false;
                    }
                
                    object[] content = new object[] { TeamID.Red , BoardManager.instance._onBoardB.Count * GameFlowManager.instance.hpLosePerHero}; 
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };  
                    PhotonNetwork.RaiseEvent(PhotonEvent.OnRoundEnd, content, raiseEventOptions, SendOptions.SendReliable);
                    return;
                }
                if ( BoardManager.instance._onBoardB.Count == 0)
                {
                    BoardManager.instance._IsEnd = true;
                    Debug.Log("Blue win round");
                    foreach (var hero in BoardManager.instance.AllHeroes())
                    {
                        hero._heroBT.enabled = false;
                    }
                
                    object[] content = new object[] { TeamID.Blue , BoardManager.instance._onBoardA.Count * GameFlowManager.instance.hpLosePerHero}; 
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };  
                    PhotonNetwork.RaiseEvent(PhotonEvent.OnRoundEnd, content, raiseEventOptions, SendOptions.SendReliable);
                }
            }
            
          
        }
        
       
      
        
       
        
    }

}

public enum TeamID
{
    Blue = 0,
    Red = 1,
}


[Serializable]
public struct HeroStats
{
    [SerializeField] public string Name;
    [SerializeField] public string Description;
    
    [SerializeField] public int Rarity;
    [SerializeField] public int Level;
    [SerializeField] public int MaxLevel;
    
    [SerializeField] public string CardId;
    [SerializeField] public int BlueTeamLevel;
    [SerializeField] public int RedTeamLevel;
    
    
    [SerializeField] public string Species;
    [SerializeField] public string Class;
    
    [SerializeField] public float Dmg;
    [SerializeField] public float Hp;
    [SerializeField] public float SkillDmg;
    [SerializeField] public int MaxMana;
    [SerializeField] public float AtkSpeed;
    [SerializeField] public int AtkRange;
    [SerializeField] public float MoveSpeed;
}

using System;
using System.Collections;
using System.Collections.Generic;
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
    
    [Header("Target")]
    [SerializeField] public Hero Target;
    [SerializeField] public int TargetPosX;
    [SerializeField] public int TargetPosY;
    
    [Header("Stats")]
    [SerializeField] public HeroStats HeroStats;

    [Header("Axie")]
    [SerializeField] private AxieSpawner _axieSpawner;
    [SerializeField] private AxieFigureController _axieFigureController;
    [SerializeField] public HeroHUD _heroHUD;
    [SerializeField] public HeroVFXController _heroVFXController;
    [SerializeField] public HeroBT _heroBT;


    [Header("Profile Config")]
    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMap;
    
    public void InitHero(TeamID teamID, string heroID, int level)
    {
        TeamID = teamID;
        HeroID = heroID;
        Level = level;
        _axieSpawner.Init(this);
        _heroHUD.SetLevel(Level);

        var config = _heroProfileConfigMap.GetValueFromKey(heroID);
        HeroStats = config.HeroStats;
    }

    public void LevelUp()
    {
        Level++;
        _heroHUD.SetLevel(Level);
    }


    public void OnDamage(float dmg)
    {
        HeroStats.Hp -= dmg;
        if (HeroStats.Hp  <= 0)
        {
            Dead();
        }
    }
    
    private void Dead()
    {
        BoardManager.instance._allHeros.Remove(this);
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
       
       
        
    }

}

public enum TeamID
{
    Blue = 0,
    Red = 1,
}


[Serializable]
public class HeroStats
{
    [SerializeField] public string Name;
    [SerializeField] public string Description;
    
    [SerializeField] public string Class;
    [SerializeField] public string Species;
    
    [SerializeField] public float Dmg;
    [SerializeField] public float Hp;
    [SerializeField] public float AtkSpeed;
    [SerializeField] public int AtkRange;
    [SerializeField] public float MoveSpeed;
}

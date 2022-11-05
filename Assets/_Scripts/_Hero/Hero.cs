using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hero : MonoBehaviour
{
    [Header("ID")]
    [SerializeField] public HeroID HeroID;
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
    [SerializeField] public float Dmg;
    [SerializeField] public float Hp;
    [SerializeField] public float AtkSpeed;
    [SerializeField] public int AtkRange;
    [SerializeField] public float MoveSpeed;

    [Header("Axie")]
    [SerializeField] private AxieSpawner _axieSpawner;
    [SerializeField] private AxieFigureController _axieFigureController;
    [SerializeField] public HeroHUD _heroHUD;
    [SerializeField] public HeroVFXController _heroVFXController;
    [SerializeField] public HeroBT _heroBT;
    
    
    private void OnDestroy()
    {
       
    }

    public void InitHero(TeamID teamID, HeroID heroID, int level)
    {
        TeamID = teamID;
        HeroID = heroID;
        Level = level;
        _axieSpawner.Init(this);
        _heroHUD.SetLevel(Level);
    }

    public void LevelUp()
    {
        Level++;
        _heroHUD.SetLevel(Level);
    }


    public void OnDamage(float dmg)
    {
        Hp -= dmg;
        if (Hp <= 0)
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

public enum HeroID
{
    A,
    B,
    C,
    D
}

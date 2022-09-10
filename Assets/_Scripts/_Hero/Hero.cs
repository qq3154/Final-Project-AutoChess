using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Stats")]
    [SerializeField] private float Dmg;
    [SerializeField] public float Hp;
    [SerializeField] public float AtkSpeed;
    [SerializeField] public float MoveSpeed;


    
    
    private void OnDestroy()
    {
       
    }

    public void InitHero(TeamID teamID, HeroID heroID, int level)
    {
        this.TeamID = teamID;
        this.HeroID = heroID;
        this.Level = level;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroData : MonoBehaviour
{
    [Header("ID")]
    [SerializeField] public HeroID HeroID; 
    [SerializeField] public TeamID TeamID; 
    
    [Header("Position")]
    [SerializeField] public int PosX;  
    [SerializeField] public int PosY;
    
    [Header("Target")]
    [SerializeField] public HeroData Target;

    [Header("Stats")]
    [SerializeField] private float Dmg;
    [SerializeField] public float Hp;
    [SerializeField] public float AtkSpeed;
    [SerializeField] public float MoveSpeed;


    private void OnDestroy()
    {
        if (this.TeamID == TeamID.Blue)
        {
            
        }
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

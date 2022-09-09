using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoSingleton<BoardManager>
{
    [SerializeField] private int _column;
    [SerializeField] private int _row;

    public  List<HeroData> _heroPositions;
    public  List<HeroData> _teamA;
    public  List<HeroData> _teamB;
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventID
{
    None = 0,
    SetupBoard,	
    OnBoardSwap,	
    OnViewSwap,
    OnViewSwapBack,
    OnBoardFindMatchWhenSwap,
    OnBoardContinueFindMatches,
    OnViewSimulate,	
    OnMoveSuccessful,
    OnEndLevel,
    OnDestroyDot,
    OnBombExplode,
    OnCombo,
	
}
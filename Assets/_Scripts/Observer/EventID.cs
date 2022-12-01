using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventID
{
    None = 0,
    OnGamePlayStart,
    OnWaveStart,
    OnSelectCardPhaseStart,
    OnSelectPositionPhaseStart,
    OnFightStart,
    
    OnAddHeroToBoard,
    OnRemoveHeroFromBoard,
    
    //
    OnLogin,
	
}
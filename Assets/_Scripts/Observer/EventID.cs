using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventID
{
    None = 0,
    OnGamePlayStart,
  
    OnSelectCardPhaseStart,
    OnSelectPositionPhaseStart,
    OnFightStart,
    
    OnAddHeroToBoard,
    OnRemoveHeroFromBoard,
    
    //
    OnLogin,
	
}

public static class PhotonEvent
{
	public static byte OnGameplayStart = 1;
	public static byte OnSetOpponentName = 2;
	public static byte OnSelectCardPhaseStart = 3;
	public static byte OnInitCards = 4;
	public static byte OnSelectCard = 5;
	public static byte OnMoveHeroToBench = 6;
	public static byte OnMoveHeroToBoard = 7;
	
}
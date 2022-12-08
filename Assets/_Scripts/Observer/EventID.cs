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
    
   
}

public static class PhotonEvent
{
	public static byte OnGameplayStart = 1;
	public static byte OnSetOpponentInfomation = 2;
	public static byte OnSelectCardPhaseStart = 3;
	public static byte OnInitCards = 4;
	public static byte OnSelectCard = 5;
	public static byte OnMoveHeroToBench = 6;
	public static byte OnMoveHeroToBoard = 7;
	public static byte OnHeroMove = 8;
	public static byte OnHeroFindTarget = 9;
	public static byte OnHeroFaceTarget = 10;
	public static byte OnHeroNormalAttack = 11;
	public static byte OnHeroUseUltimate = 12;
	
	public static byte OnRoundEnd = 13;
	public static byte OnMatchEnd = 14;
	public static byte OnAFK = 15;



}
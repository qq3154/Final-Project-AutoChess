using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoSingleton<GameFlowManager>
{
    public GameState gameState;
    public GamePlayState GamePlayState;
}

public enum GameState
{
    OnMenu,
    OnGamePlay,
    OnPause
}

public enum GamePlayState
{
    OnSelectCard,
    OnFight,
}
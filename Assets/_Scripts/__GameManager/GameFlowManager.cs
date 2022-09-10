using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameFlowManager : MonoSingleton<GameFlowManager>
{
    public GameState gameState;
    [FormerlySerializedAs("GamePlayState")] public GamePlayState gamePlayState;

    public int waveID;
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

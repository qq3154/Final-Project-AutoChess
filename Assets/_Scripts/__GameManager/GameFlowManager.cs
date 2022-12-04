using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameFlowManager : MonoSingleton<GameFlowManager>
{
    public bool isConnect =false;
    public GameState gameState;
    public GamePlayState gamePlayState;

    public int waveID;

    public TeamID playerTeam;


    #region Debug methods

    public void SetPlayerTeamBlue()
    {
        playerTeam = TeamID.Blue;
    }
    
    public void SetPlayerTeamRed()
    {
        playerTeam = TeamID.Red;
    }

    #endregion
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

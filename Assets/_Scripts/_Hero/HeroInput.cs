using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInput : MonoBehaviour
{
    [SerializeField] private Hero hero;
    
    private void OnMouseDown()
    {
       
        if (BoardManager.instance._IsLock)
        {
            return;
        }
        
        if (hero.TeamID == GameFlowManager.instance.playerTeam)
        {
            Debug.Log("select hero");
            BoardManager.instance.SelectHero(hero);
        }
    }
}

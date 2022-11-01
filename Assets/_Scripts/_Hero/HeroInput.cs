using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInput : MonoBehaviour
{
    [SerializeField] private Hero hero;
    
    private void OnMouseDown()
    {
        Debug.Log("select hero");
        if (hero.TeamID == GameFlowManager.instance.playerTeam)
        {
            BoardManager.instance.SelectHero(hero);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchSlot : MonoBehaviour
{
    [SerializeField] private TeamID _teamID;
    [SerializeField] private Hero _hero;

    [SerializeField] private int index;

    [SerializeField] private CircleCollider2D _collider2D;
    
    public bool isUse;


    public void SetHero(Hero hero)
    {
        _hero = hero;
        isUse = true;
        _collider2D.enabled = false;
    }

    public Hero GetHero()
    {
        return _hero;
    }

    public void RemoveHero()
    {
        isUse = false;
        _collider2D.enabled = true;
    }
    
    private void OnMouseDown()
    {
        if (BoardManager.instance._IsLock)
        {
            return;
        }
        
        if (_teamID == GameFlowManager.instance.playerTeam)
        {
            Debug.Log("select bench" + index);
            BoardManager.instance.SelectBench(GameFlowManager.instance.playerTeam, index);
        }
    }
}

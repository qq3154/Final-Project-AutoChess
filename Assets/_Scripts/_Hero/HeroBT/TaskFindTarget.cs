using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskFindTarget : Node
{
    private Hero _hero;
    
    public TaskFindTarget(Hero hero)
    {
        this._hero = hero;
    }
    
    public override NodeState Evaluate()
    {
        List<Hero> enemyTeam;
        if (_hero.Target == null)
        {
            if (_hero.TeamID == TeamID.Blue)
            {
                enemyTeam = BoardManager.instance._teamB;
            }
            else
            {
                enemyTeam = BoardManager.instance._teamA;
            }

            if (enemyTeam == null || enemyTeam.Count ==0)
            {
                return NodeState.FAILURE;
            }

            foreach (var enemy in enemyTeam)
            {
                this._hero.Target = enemy;
                return NodeState.SUCCESS;
                
            }
            
        }
        
        return state;
    }
}

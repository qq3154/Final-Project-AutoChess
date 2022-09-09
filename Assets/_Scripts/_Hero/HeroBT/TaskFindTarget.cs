using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskFindTarget : Node
{
    private HeroData _heroData;
    
    public TaskFindTarget(HeroData heroData)
    {
        this._heroData = heroData;
    }
    
    public override NodeState Evaluate()
    {
        List<HeroData> enemyTeam;
        if (_heroData.Target == null)
        {
            if (_heroData.TeamID == TeamID.Blue)
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
                this._heroData.Target = enemy;
                return NodeState.SUCCESS;
                
            }
            
        }
        
        return state;
    }
}

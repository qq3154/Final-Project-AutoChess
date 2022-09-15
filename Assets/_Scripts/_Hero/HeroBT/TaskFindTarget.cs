using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskFindTarget : Node
{
    private Hero _hero;
    
    public TaskFindTarget(Hero hero)
    {
        _hero = hero;
    }
    
    public override NodeState Evaluate()
    {
        List<Hero> enemyTeam;
        int distance = BoardManager.instance._X * BoardManager.instance._Y;
        
        if (_hero.Target  == null)
        {
            enemyTeam = _hero.TeamID == TeamID.Blue ? BoardManager.instance._teamB : BoardManager.instance._teamA;

            if (enemyTeam == null || enemyTeam.Count ==0)
            {
                return NodeState.FAILURE;
            }

            state = NodeState.FAILURE;

            foreach (var enemy in enemyTeam)
            {
                if (enemy == null)
                {
                    continue;
                }
                
                var findPath = new FindShortestPath();
                findPath.Setup(_hero, enemy);
                if (findPath.distance < distance)
                {
                    distance = findPath.distance;
                    _hero.Target = enemy;
                    state = NodeState.SUCCESS;
                }
            }
        }

        return  state;
    }
}

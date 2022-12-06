using System;
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
        
        enemyTeam = _hero.TeamID == TeamID.Blue ? BoardManager.instance._onBoardB : BoardManager.instance._onBoardA;

        if (enemyTeam == null || enemyTeam.Count ==0)
        {
            return NodeState.FAILURE;
        }
        
        if (_hero.Target  == null)
        {
            state = NodeState.FAILURE;

            foreach (var enemy in enemyTeam)
            {
                if (enemy == null)
                {
                    continue;
                }

                for (int i = 0; i < BoardManager.instance._X; i++)
                {
                    for (int j = 0; j < BoardManager.instance._Y; j++)
                    {
                        int range = Mathf.Max(Mathf.Abs(enemy.PosX - i), Mathf.Abs(enemy.PosY - j));
                        if (range <= _hero.HeroStats.AtkRange)
                        {
                            if (_hero.PosX == i && _hero.PosY == j)
                            {
                                _hero.Target = enemy;
                                state = NodeState.SUCCESS;
                            }
                            else
                            {
                                if (BoardManager.instance.Pos[i, j] == false )
                                {
                                    var findPath = new FindShortestPath();
                                    findPath.Setup(_hero.PosX, _hero.PosY, i, j);
                                    if (findPath.distance < distance)
                                    {
                                        distance = findPath.distance;
                                        _hero.Target = enemy;
                                        _hero.TargetPosX = i;
                                        _hero.TargetPosY = j;
                                        state = NodeState.SUCCESS;
                                    }
                                }
                            }
                            
                        }
                    }
                }
                
                
            }
        }
        else
        {
            int rangetoAttack = Mathf.Max(Mathf.Abs(_hero.PosX - _hero.Target.PosX), Mathf.Abs(_hero.PosY - _hero.Target.PosY));
            if (rangetoAttack <= _hero.HeroStats.AtkRange)
            {
                return NodeState.SUCCESS;
            }
            foreach (var enemy in enemyTeam)
            {
                if (enemy == null)
                {
                    continue;
                }

                for (int i = 0; i < BoardManager.instance._X; i++)
                {
                    for (int j = 0; j < BoardManager.instance._Y; j++)
                    {
                        int range = Mathf.Max(Mathf.Abs(enemy.PosX - i), Mathf.Abs(enemy.PosY - j));
                        if (range <= _hero.HeroStats.AtkRange)
                        {
                            if (_hero.PosX == i && _hero.PosY == j)
                            {
                                _hero.Target = enemy;
                                state = NodeState.SUCCESS;
                            }
                            else
                            {
                                if (BoardManager.instance.Pos[i, j] == false )
                                {
                                    var findPath = new FindShortestPath();
                                    findPath.Setup(_hero.PosX, _hero.PosY, i, j);
                                    if (findPath.distance < distance)
                                    {
                                        distance = findPath.distance;
                                        _hero.Target = enemy;
                                        _hero.TargetPosX = i;
                                        _hero.TargetPosY = j;
                                        state = NodeState.SUCCESS;
                                    }
                                }
                            }
                            
                        }
                    }
                }
                
                
            }
            
            if(distance == BoardManager.instance._X * BoardManager.instance._Y) Debug.Log("not found target"); 
        }
       

        return  state;
    }
}

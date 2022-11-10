using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Unity.VisualScripting;

public class IsUltimateReady : Node
{
    private Hero _hero;

    public IsUltimateReady(Hero hero)
    {
        _hero = hero;
    }

    public override NodeState Evaluate()
    {
        
        if (_hero.CurrentMana >= _hero.HeroStats.MaxMana)
        {
            return NodeState.SUCCESS;
        }
        

        return NodeState.FAILURE;
    }
}
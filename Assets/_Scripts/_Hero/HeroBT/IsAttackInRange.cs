using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Unity.VisualScripting;

public class IsAttackInRange : Node
{
    private Hero _hero;
    private Hero _target;

    public IsAttackInRange(Hero hero)
    {
        _hero = hero;
    }

    public override NodeState Evaluate()
    {
        
        _target = _hero.Target;
            
        

        // float distance = Mathf.Sqrt(
        //                             Mathf.Pow(Mathf.Abs(_hero.PosX - _target.PosX), 2) +
        //                             Mathf.Pow(Mathf.Abs(_hero.PosY - _target.PosY), 2)) ;

        float distance = Mathf.Max(Mathf.Abs(_hero.PosX - _target.PosX), Mathf.Abs(_hero.PosY - _target.PosY));
        
        if (distance <= _hero.HeroStats.AtkRange)
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}

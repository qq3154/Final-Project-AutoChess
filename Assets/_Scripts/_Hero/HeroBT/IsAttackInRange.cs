using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

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
        if (_target == null)
        {
            _target = _hero.Target;
            return NodeState.FAILURE;
        }

        float distance = Mathf.Sqrt(
                                    Mathf.Pow(Mathf.Abs(_hero.PosX - _target.PosX), 2) +
                                    Mathf.Pow(Mathf.Abs(_hero.PosY - _target.PosY), 2)) ;
        if (distance <= _hero.AtkRange)
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}

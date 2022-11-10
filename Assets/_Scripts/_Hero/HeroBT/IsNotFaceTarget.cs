using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Unity.VisualScripting;

public class IsNotFaceTarget : Node
{
    private Hero _hero;
    private Hero _target;

    public IsNotFaceTarget(Hero hero)
    {
        _hero = hero;
    }

    public override NodeState Evaluate()
    {
        
        _target = _hero.Target;



        if (_hero.PosX < _target.PosX && !_hero.IsFaceRight)
        {
            return NodeState.SUCCESS;
        }
        
        if (_hero.PosX > _target.PosX && _hero.IsFaceRight)
        {
            return NodeState.SUCCESS;
        }
        
        if (_hero.PosX == _target.PosX && _hero.PosY > _target.PosY && _hero.IsFaceRight)
        {
            return NodeState.SUCCESS;
        }
        
        if (_hero.PosX == _target.PosX && _hero.PosY < _target.PosY && !_hero.IsFaceRight)
        {
            return NodeState.SUCCESS;
        }
        
        

        return NodeState.FAILURE;
    }
}

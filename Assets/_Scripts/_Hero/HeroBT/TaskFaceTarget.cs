using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BehaviorTree;

public class TaskFaceTarget : Node
{
    private Hero _hero;
    private float _attackCounter = 0f;

    public TaskFaceTarget(Hero hero)
    {
        _hero = hero;
    }
    
    public override NodeState Evaluate()
    {
        _attackCounter += Time.deltaTime;
        if (_attackCounter >= 0.5f )
        {
            _hero.IsFaceRight = !_hero.IsFaceRight;
            _hero._axieFigureController.SwitchFace(_hero.IsFaceRight);
            return NodeState.SUCCESS;
        }
        
        state = NodeState.RUNNING;
        return state;
        
    }
}
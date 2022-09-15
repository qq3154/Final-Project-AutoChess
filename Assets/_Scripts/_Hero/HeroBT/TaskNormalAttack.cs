using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BehaviorTree;

public class TaskNormalAttack : Node
{
    private float _attackTime = 1f;
    private float _attackCounter = 0f;
    
    public override NodeState Evaluate()
    {
        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime)
        {
            Debug.Log("attack");
            _attackCounter = 0f;
        }
        
        state = NodeState.RUNNING;
        return state;
        
    }
}

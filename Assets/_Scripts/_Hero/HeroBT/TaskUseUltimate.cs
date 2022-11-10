using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BehaviorTree;

public class TaskUseUltimate : Node
{
    private Hero _hero;
    private float _attackCounter = 0f;

    private bool isUseUltimate = false;

    public TaskUseUltimate(Hero hero)
    {
        _hero = hero;
    }
    
    public override NodeState Evaluate()
    {
        if (!isUseUltimate)
        {
            _hero._axieFigureController.SetUseUltimate();
            
            Debug.LogError("Use Ultimate " + _hero.name);

            isUseUltimate = true;
        }
        
        _attackCounter += Time.deltaTime;
        if (_attackCounter >= 2f )
        {
            _attackCounter = 0;
            
            _hero.CurrentMana = 0;
            isUseUltimate = false;

        }


        state = NodeState.RUNNING;
        return state;
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskMovetoTarget : Node
{
    private Hero _hero;
    private Hero _target;

    private float _attackTime = 1f;
    private float _moveCounter = 0f;

    public TaskMovetoTarget(Hero hero)
    {
        _hero = hero;
    }

    public override NodeState Evaluate()
    {

        if (_target == null)
        {
            _target = _hero.Target;
            //return NodeState.FAILURE;
        }

        _moveCounter += Time.deltaTime;
        if (_moveCounter >= 1 / _hero.MoveSpeed)
        {
            var findPath = new FindShortestPath();
            findPath.Setup(_hero, _target);
            if (findPath.distance > 2)
            {
                _hero.PosX = findPath.nextMoveX;
                _hero.PosY = findPath.nextMoveY;
                _hero.transform.position = new Vector3(_hero.PosX, _hero.PosY, 0);
                _moveCounter = 0f;
                Debug.Log("move");
            }
            _moveCounter = 0f;
        }
        
        state = NodeState.RUNNING;
        return state;
    }
}

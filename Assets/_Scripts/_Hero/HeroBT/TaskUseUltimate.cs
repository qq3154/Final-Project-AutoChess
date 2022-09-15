using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TaskUseUltimate : Node
{
    public override NodeState Evaluate()
    {
        Debug.Log("use ultimate");
        return NodeState.SUCCESS;
    }
}
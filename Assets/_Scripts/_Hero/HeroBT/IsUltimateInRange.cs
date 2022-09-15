using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class IsUltimateInRange : Node
{
    public override NodeState Evaluate()
    {
        return NodeState.FAILURE;
    }
}
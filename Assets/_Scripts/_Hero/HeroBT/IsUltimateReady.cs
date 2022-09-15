using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class IsUltimateReady : Node
{
    public override NodeState Evaluate()
    {
        return NodeState.FAILURE;
    }
}

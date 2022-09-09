using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using Tree = BehaviorTree.Tree;

public class HeroBT : Tree
{
    [SerializeField] private HeroData _heroData;
    
    protected override Node SetupTree()
    {
        // Node root = new Selector(new List<Node>
        // {
        //     new Sequence(new List<Node>
        //     {
        //         new CheckEnemyInAttackRange(transform),
        //         new TaskAttack(transform),
        //     }),
        //     new Sequence(new List<Node>
        //     {
        //         new CheckEnemyInFOVRange(transform),
        //         new TaskGoToTarget(transform),
        //     }),
        //     new TaskPatrol(transform, waypoints),
        // });

        Node root = new Sequence(new List<Node>
        {
            new TaskFindTarget(_heroData),
            // new Selector(new List<Node>
            // {
            //     new Sequence(new List<Node>(
            //     {
            //         new IsUltimateReady(),
            //         new Selector(new List<Node>(
            //         {
            //             new Selector(new List<Node>(
            //             {
            //                 new IsInUltimateRange(),
            //                 new TaskUseUltimate(),
            //             }),
            //             new TaskMovetoTarget(),
            //         })
            //     }),
            //     new Sequence(new List<Node>(
            //     {
            //         new IsInAttackRange(),
            //         new TaskNormalAttack(),
            //     }),
            //     new Sequence(new List<Node>(
            //     {
            //         new IsNotInAttackRange(),
            //         new TaskMovetoTarget(),
            //     }),
            // })
        });
    


    return root;
    }
}
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.Serialization;
using Tree = BehaviorTree.Tree;

public class HeroBT : Tree
{
    [SerializeField] private Hero hero;
    
    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node>
        {
            new TaskFindTarget(hero),
            new Selector( new List<Node>
            {
                
                new Sequence(new List<Node>
                {
                    new IsNotFaceTarget(hero),
                    new TaskFaceTarget(hero)
                }),
                
                new Sequence(new List<Node>
                {
                    new IsUltimateReady(hero),
                    new TaskUseUltimate(hero)
                }),
                
                new Sequence(new List<Node>
                {
                    new IsAttackInRange(hero),
                    new TaskNormalAttack(hero)
                }),
                new TaskMovetoTarget(hero),
            })
            
           
            // new Selector(new List<Node>
            // {
                // new Sequence(new List<Node>
                // {
                //     new IsUltimateReady(),
                //     new Selector(new List<Node>
                //     {
                //         new Sequence(new List<Node>
                //         {
                //             new IsUltimateInRange(),
                //             new TaskUseUltimate(),
                //         }),
                //        // new TaskMovetoTarget(hero),
                //     })
                // }),
                
              
                
                // new Sequence(new List<Node>(
                // {
                //     new IsInAttackRange(),
                //     new TaskNormalAttack(),
                // }),
                // new Sequence(new List<Node>(
                // {
                //     new IsNotInAttackRange(),
                //     new TaskMovetoTarget(),
                // }),
            // })
        });
    


    return root;
    }
}
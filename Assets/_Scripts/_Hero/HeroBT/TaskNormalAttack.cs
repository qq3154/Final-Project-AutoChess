using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BehaviorTree;

public class TaskNormalAttack : Node
{
    private Hero _hero;
    private float _attackCounter = 0f;
    private bool isAttack = false;

    public TaskNormalAttack(Hero hero)
    {
        _hero = hero;
    }
    
    public override NodeState Evaluate()
    {
        if (!isAttack)
        {
            isAttack = true;
            
            _hero._axieFigureController.SetAttack();
            
            //melee attack
            if (_hero.HeroStats.AtkRange == 1)
            {
                if (_hero.Target != null)
                {
                    _hero.Target.OnDamage(_hero.HeroStats.Dmg);
                }
            }
            else
            {
                RangeAttackBullet bullet = GameObject.Instantiate(_hero._rangeAttackBulletPref);
                bullet.Init(_hero, _hero.Target, _hero.HeroStats.Dmg);
            }
            Debug.Log( _hero.name + " attack " + _hero.Target.name);
        }
        
        _attackCounter += Time.deltaTime;
        if (_attackCounter >= 0.25f +  1/_hero.HeroStats.AtkSpeed)
        {
            _attackCounter = 0;

            isAttack = false;

        }
        
        state = NodeState.RUNNING;
        return state;
        
    }
}

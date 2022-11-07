using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BehaviorTree;

public class TaskNormalAttack : Node
{
    private Hero _hero;
    private float _attackCounter = 0f;

    public TaskNormalAttack(Hero hero)
    {
        _hero = hero;
    }
    
    public override NodeState Evaluate()
    {
        _attackCounter += Time.deltaTime;
        if (_attackCounter >= 1 / _hero.HeroStats.AtkSpeed)
        {
            Debug.Log( _hero.name + " attack " + _hero.Target.name);
            
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
           
            _attackCounter = 0f;
        }
        
        state = NodeState.RUNNING;
        return state;
        
    }
}

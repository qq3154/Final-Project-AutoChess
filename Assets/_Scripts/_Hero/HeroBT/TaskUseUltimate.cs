using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using BehaviorTree;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

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
            
            object[] content = new object[] {_hero.PosX, _hero.PosY, _hero.Target.PosX, _hero.Target.PosY, _hero.HeroStats.SkillDmg}; 
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };  
            PhotonNetwork.RaiseEvent(PhotonEvent.OnHeroUseUltimate, content, raiseEventOptions, SendOptions.SendReliable);
            
            _hero._axieFigureController.SetUseUltimate();
            _hero.Target.OnDamage(_hero.HeroStats.SkillDmg);
            
            Debug.LogWarning("Use Ultimate " + _hero.name);

            isUseUltimate = true;
        }
        
        _attackCounter += Time.deltaTime;
        if (_attackCounter >= 1f )
        {
            _attackCounter = 0;
            
            _hero.CurrentMana = 0;
            isUseUltimate = false;

        }


        state = NodeState.RUNNING;
        return state;
        
    }
}
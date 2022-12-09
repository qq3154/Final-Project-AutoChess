using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class HeroPhotonEvent : MonoBehaviour, IOnEventCallback
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonEvent.OnHeroMove)
        {
            object[] data = (object[])photonEvent.CustomData;
            int posX = (int)data[0];
            int posY = (int)data[1];
            int nextPosX = (int)data[2];
            int nextPosY = (int)data[3];
            //
            // MoveHeroToBench(teamId, GetHeroByPosition(posX, posY), index);

            var hero = BoardManager.instance.GetHeroByPosition(posX, posY);
            if(hero == null) return;
            
            hero.PosX = nextPosX;
            hero.PosY = nextPosY;
                
            hero.transform.DOMove(new Vector3(hero.PosX, hero.PosY, 0), 1/hero.HeroStats.MoveSpeed)
                .SetDelay(0)
                .SetEase(Ease.InFlash)
                .OnStart(() => DoOnStartMove(hero));
            
            Debug.Log("on hero move");
        }
        
        if (eventCode == PhotonEvent.OnHeroFaceTarget)
        {
            object[] data = (object[])photonEvent.CustomData;
            int posX = (int)data[0];
            int posY = (int)data[1];
          

            var hero = BoardManager.instance.GetHeroByPosition(posX, posY);
            if(hero == null) return;
            
            hero.IsFaceRight = !hero.IsFaceRight;
            hero._axieFigureController.SwitchFace(hero.IsFaceRight);
                
            
            
            Debug.Log("on hero face target");
        }
        
        if (eventCode == PhotonEvent.OnHeroNormalAttack)
        {
            object[] data = (object[])photonEvent.CustomData;
            int posX = (int)data[0];
            int posY = (int)data[1];
            int targetPosX = (int)data[2];
            int targetPosY = (int)data[3];
            float dmg = (float)data[4];
            
            var hero = BoardManager.instance.GetHeroByPosition(posX, posY);
            var target =  BoardManager.instance.GetHeroByPosition(targetPosX, targetPosY);

            if (hero == null) return;
            if (target == null) return;
            
            hero.CurrentMana += 5;
            hero._heroHUD.SetManaValue(hero.CurrentMana);
            hero._axieFigureController.SetAttack();
            
            //melee attack
            if (hero.HeroStats.AtkRange == 1)
            {
                if (target != null)
                {
                    target.OnDamage(dmg);
                }
            }
            else
            {
                RangeAttackBullet bullet = GameObject.Instantiate(hero._rangeAttackBulletPref);
                bullet.Init(hero, target, dmg);
            }
            
            Debug.Log("on hero normal attack");
        }
        
        if (eventCode == PhotonEvent.OnHeroUseUltimate)
        {
            object[] data = (object[])photonEvent.CustomData;
            int posX = (int)data[0];
            int posY = (int)data[1];
            int targetPosX = (int)data[2];
            int targetPosY = (int)data[3];
            float dmg = (float)data[4];
            
            var hero = BoardManager.instance.GetHeroByPosition(posX, posY);
            var target =  BoardManager.instance.GetHeroByPosition(targetPosX, targetPosY);

            if (hero == null) return;
            if (target == null) return;

            hero._axieFigureController.SetUseUltimate();
            target.OnDamage(dmg);
            
            
            Debug.Log("on hero use ultimate");
        }
    }
    
    private void DoOnStartMove(Hero hero)
    {
        hero._axieFigureController.SetRun(hero.HeroStats.MoveSpeed);
    }

    
    
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    
}

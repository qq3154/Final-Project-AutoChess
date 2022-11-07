using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class RangeAttackBullet : MonoBehaviour
{
    [SerializeField] private float speed = 20;
    private Hero _target;

    public void Init(Hero shooter, Hero target, float damage)
    {

        var duration = Vector2.Distance(target._bulletRoot.transform.position, this.transform.position) / speed; 
      
        transform.position = shooter._bulletRoot.transform.position;
        
        transform.DOMove(target._bulletRoot.transform.position, duration)
            .OnComplete(() => DoOnComplete(target, damage));

    }
    
    private void DoOnComplete(Hero target, float damage)
    {
        if (Vector2.Distance(target._bulletRoot.transform.position, this.transform.position) > 0.05)
        {
            var duration = Vector2.Distance(target._bulletRoot.transform.position, this.transform.position) / speed; 
            transform.DOMove(target._bulletRoot.transform.position, duration)
                .OnComplete(() => DoOnComplete(target, damage));
        }
        else
        {
            if (target != null)
            {
                target.OnDamage(damage);
            }
            Destroy(gameObject);
        }
        
       
    }
}

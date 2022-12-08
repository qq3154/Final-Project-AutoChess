using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logout : MonoBehaviour
{
    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMap;

    public void OnLogOut()
    {
        UserManager.instance.auth = String.Empty;
        UserManager.instance.id = String.Empty;
        UserManager.instance.username = String.Empty;
        UserManager.instance.password = String.Empty;
        UserManager.instance.fullName = String.Empty;
        UserManager.instance.email = String.Empty;
        UserManager.instance.gold = 0;

        foreach (var config in _heroProfileConfigMap.list)
        {
            config.heroConfig.HeroStats.Level = 1;
            config.heroConfig.HeroStats.CardId = String.Empty;
        }
     
        
    }
}

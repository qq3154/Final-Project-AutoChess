using System;
using System.Collections;
using System.Collections.Generic;
using Observer;
using UnityEngine;

public class BoardStrategy : MonoBehaviour
{
   [SerializeField] private TeamID _teamID;
   [SerializeField] private StrategyItem _strategyItemPref;
   private void Awake()
   {
      this.RegisterListener(EventID.OnAddHeroToBoard,(param) => CountStrategies((Hero)param) );
      this.RegisterListener(EventID.OnRemoveHeroFromBoard,(param) => CountStrategies((Hero)param) );
   }

   private void CountStrategies(Hero hero)
   {
      if (hero.TeamID != _teamID)
      {
         return;
      }
      
      List<Hero> uniqueHeroes = new List<Hero>();
      foreach (var heroOnBoard in BoardManager.instance.PlayerOnBoard(_teamID))
      {
         if (!uniqueHeroes.Exists((s => s.HeroID == heroOnBoard.HeroID)))
         {
            uniqueHeroes.Add(heroOnBoard);
         }
      }

      Dictionary<string, int> strategies = BoardManager.instance.PlayerStrategies(_teamID);
      strategies.Clear();
      foreach (var uniqueHero in uniqueHeroes)
      {
         if (uniqueHero.HeroStats.Class != String.Empty)
         {
            if (strategies.ContainsKey(uniqueHero.HeroStats.Class))
            {
               strategies[uniqueHero.HeroStats.Class]++;
            }
            else
            {
               strategies.Add(uniqueHero.HeroStats.Class,1);
            }
         }
      
         if (uniqueHero.HeroStats.Species != String.Empty)
         {
            if (strategies.ContainsKey(uniqueHero.HeroStats.Species))
            {
               strategies[uniqueHero.HeroStats.Species]++;
            }
            else
            {
               strategies.Add(uniqueHero.HeroStats.Species,1);
            }
         }
      }
      

      ShowUI(strategies);
   }

   private void ShowUI(Dictionary<string, int> strategies)
   {
      foreach (Transform child in this.transform) {
         Destroy(child.gameObject);
      }

      foreach (var strategy in strategies)
      {
         StrategyItem strategyItem = Instantiate(_strategyItemPref, transform);
         strategyItem.SetStrategy(strategy.Key, strategy.Value);
      }
   }
}

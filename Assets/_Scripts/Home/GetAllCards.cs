using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllCards : MonoBehaviour
{
    [SerializeField] private HeroProfileConfigMap _heroProfileConfigMap;
    // Start is called before the first frame update
    void Start()
    {
        SendGetCardsRequest();
    }

    public void SendGetCardsRequest()
    {
        GetCardsRequest();
    }
    
    private async void GetCardsRequest()
    {
        var response = await ApiRequest.instance.SendGetCardsRequest();
        
        if (response.success)
        {
            var configs = _heroProfileConfigMap.list;

            foreach (var config in configs)
            {
                foreach (var card in response.cards)
                {
                    if (card.name == config.heroConfig.HeroStats.Name)
                    {
                        config.heroConfig.HeroStats.Level = card.level;
                        config.heroConfig.HeroStats.CardId = card._id;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Get user profile fail");
        }
    }
}
